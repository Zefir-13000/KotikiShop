const gallery = document.querySelector('.gallery');
const slides = document.querySelectorAll('.slide');
const dots = document.querySelectorAll('.dot');

let currentSlide = 0;
const slideCount = slides.length;

let autoplayInterval = null;
let autoplayIsActive = false;

let userInteractionTimeout = null;

let touchStartX = 0;
let touchEndX = 0;
let isDragging = false;
let startTransform = 0;
let currentTransform = 0

function updateGallery() {
    gallery.style.transform = `translateX(-${currentSlide * 100}%)`;

    dots.forEach((dot, index) => {
        dot.classList.toggle('active', index === currentSlide);
    });
}

dots.forEach((dot, index) => {
    dot.addEventListener('click', () => {
        currentSlide = index;
        updateGallery();
        handleUserInteraction();
    });
});

function startAutoplay() {
    stopAutoplay();
    autoplayInterval = setInterval(function () {
        currentSlide = (currentSlide + 1) % slideCount;
        updateGallery();
    }, 3000);
}

function stopAutoplay() {
    if (autoplayInterval) {
        clearInterval(autoplayInterval);
        autoplayInterval = null;
    }
}

function handleUserInteraction() {
    stopAutoplay();

    if (userInteractionTimeout) {
        clearTimeout(userInteractionTimeout);
    }

    userInteractionTimeout = setTimeout(() => {
        startAutoplay();
    }, 7000);
}

function handleDragStart(event) {
    isDragging = true;
    startX = event.type.includes('mouse') ? event.clientX : event.touches[0].clientX;
    startTransform = currentSlide * -100;
    gallery.style.transition = 'none';
    gallery.style.cursor = 'grabbing';
    handleUserInteraction();
}

function handleDrag(event) {
    if (!isDragging) return;

    event.preventDefault();
    currentX = event.type.includes('mouse') ? event.clientX : event.touches[0].clientX;
    const diff = currentX - startX;
    const newTransform = startTransform + (diff / gallery.offsetWidth * 100);

    if (newTransform < (startTransform - 100) || newTransform > (startTransform + 100)) return;

    currentTransform = newTransform;
    gallery.style.transform = `translateX(${newTransform}%)`;
}

function handleDragEnd() {
    if (!isDragging) return;
    isDragging = false;
    gallery.style.transition = 'transform 0.3s ease-out';
    gallery.style.cursor = 'grab';

    const diff = currentTransform - startTransform;

    if (Math.abs(diff) > 20) {
        if (diff > 0 && currentSlide > 0) {
            currentSlide--;
        } else if (diff < 0 && currentSlide < slideCount - 1) {
            currentSlide++;
        }
    }

    updateGallery();
}

// Touch Events
gallery.addEventListener('touchstart', handleDragStart, { passive: true });
gallery.addEventListener('touchmove', handleDrag, { passive: false });
gallery.addEventListener('touchend', handleDragEnd);
gallery.addEventListener('touchcancel', handleDragEnd);

// Mouse Events
gallery.addEventListener('mousedown', handleDragStart);
gallery.addEventListener('mousemove', handleDrag);
gallery.addEventListener('mouseup', handleDragEnd);
gallery.addEventListener('mouseleave', handleDragEnd);

// Prevent default drag behavior
gallery.addEventListener('dragstart', (e) => e.preventDefault());

startAutoplay();