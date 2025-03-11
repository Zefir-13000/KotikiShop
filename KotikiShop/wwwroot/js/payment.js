let account;
let totalCost;
let finalCost = 0;

const DeliveryMethod = Object.freeze({
    Courier: 0,
    Coords: 1,
    Virtual: 2
});
let deliveryMethod = DeliveryMethod.Virtual;

function postRedirect(url, data) {
    const form = document.createElement("form");
    form.method = "POST";
    form.action = url;

    for (const key in data) {
        if (data.hasOwnProperty(key)) {
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = key;
            input.value = data[key];
            form.appendChild(input);
        }
    }

    document.body.appendChild(form);
    form.submit();
}

function waitForTransaction(txHash) {
    return new Promise((resolve) => {
        const checkTransaction = async () => {
            const receipt = await ethereum.request({
                method: "eth_getTransactionReceipt",
                params: [txHash],
            });

            if (receipt && receipt.status) {
                resolve();
            } else {
                setTimeout(checkTransaction, 2000); // Retry every 2 seconds
            }
        };

        checkTransaction();
    });
}


$(document).ready(function () {
    fetch("/api/cart/sum").then(function (res) {
        res.json().then(function (res1) {
            totalCost = res1["total"];
            finalCost = totalCost;
            let button_confirm = $("#confirm-payment")[0];
            button_confirm.textContent = `Pay ${finalCost} ETH`;
        });
    });

    const deliveryOptions = document.querySelectorAll("input[name='delivery']");
    const priceDisplay = document.getElementById("order-price");
    function updatePrice() {
        let price = 0;
        const selectedOption = document.querySelector("input[name='delivery']:checked");
        let button_confirm = $("#confirm-payment")[0];

        if (selectedOption) {
            switch (selectedOption.id) {
                case "courier":
                    deliveryMethod = DeliveryMethod.Courier;
                    price = 0.005;
                    break;
                case "coordinates":
                    deliveryMethod = DeliveryMethod.Coords;
                    price = 0.001;
                    break;
                case "virtual":
                    deliveryMethod = DeliveryMethod.Virtual;
                    price = 0;
                    break;
            }
        }

        finalCost = totalCost + price;
        button_confirm.textContent = `Pay ${finalCost} ETH`;
        priceDisplay.textContent = `До сплати: ${finalCost} ETH`;
    }

    deliveryOptions.forEach(option => {
        option.addEventListener("change", updatePrice);
    });
});

$("#connect-metamask").click(() => {
    let button_connect = $("#connect-metamask")[0];
    let button_confirm = $("#confirm-payment")[0];
    let label = $("#label-error")[0];

    try {
        ethereum.request({ method: 'eth_requestAccounts' }).then(accounts => {
            account = accounts[0];
            console.log(account);
            button_connect.textContent = account;
            ethereum.request({ method: 'eth_getBalance', params: [account, 'latest'] }).then(result => {
                console.log(result);

                let wei = parseInt(result, 16);
                let balance = wei / (10 ** 18);
                console.log(balance);

                button_connect.disabled = true;
                button_confirm.hidden = false;
                label.hidden = true;
                // 0x11f079ED7e34c64B615BeA6414E25fA1C7534871
            });
        });
    }
    catch(error) {
        label.hidden = false;
    };
});

$("#confirm-payment").click(() => {
    let transactionParams = {
        to: "0x11f079ED7e34c64B615BeA6414E25fA1C7534871",
        from: account,
        value: "0x" + (finalCost * 10 ** 18).toString(16)
    };

    console.log(transactionParams);
    ethereum.request({ method: "eth_sendTransaction", params: [transactionParams] })
        .then(txHash => {
            console.log("Transaction Hash:", txHash);

            waitForTransaction(txHash).then(() => {
                $.post("/api/cart/submit", { txHash })
                    .done(response => {
                        alert("✅ Payment successful! Redirecting to the main page...");
                        window.location.href = "/"; // Redirect to main page
                    })
                    .fail(error => {
                        alert("❌ Payment failed! " + (error.responseJSON?.message || "Please try again."));
                    });
            });
        })
        .catch(error => {
            console.error("Transaction failed", error);
            alert("❌ Transaction rejected or failed!");
        });
});