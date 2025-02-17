// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// MITA IMAGE
const mitaImage = document.getElementById('mita-clickable');
var audio = new Audio('/audio/mita-sound.mp3');

mitaImage.addEventListener('click', function () {
    if (!this.classList.contains('clicked')) {
        this.classList.add('clicked');
        audio.play();
    };
});

mitaImage.addEventListener('mouseout', function () {
    if (this.classList.contains('clicked')) {
        setTimeout(() => {
            this.classList.remove('clicked');
        }, 100);
    }
});