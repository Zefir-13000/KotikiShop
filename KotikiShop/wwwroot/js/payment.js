$("#connect-metamask").click(() => {
    let account;
    ethereum.request({ method: 'eth_requestAccounts' }).then(accounts => {
        account = accounts[0];
        console.log(account);
        $(this).text(account);

        ethereum.request({ method: 'eth_getBalance', params: [account, 'latest'] }).then(result => {
            console.log(result);

            let wei = parseInt(result, 16);
            let balance = wei / (10 ** 18);
            console.log(balance);

            // 0x11f079ED7e34c64B615BeA6414E25fA1C7534871
        });
    });
});