const controller = document.querySelector('.banner');
const slides = document.querySelectorAll('.slide');
const dotsContainer = document.querySelector('.dots-container');

// Clone first and last slides
const firstSlideClone = slides[0].cloneNode(true);
const lastSlideClone = slides[slides.length - 1].cloneNode(true);

// Add clones to DOM
controller.appendChild(firstSlideClone);
controller.insertBefore(lastSlideClone, slides[0]);

function createDots() {
    slides.forEach((_, index) => {
        const dot = document.createElement('div');
        dot.className = 'dot';
        if (index === 0) dot.classList.add('active');
        dotsContainer.appendChild(dot);
    });
}

createDots();

const dots = document.querySelectorAll('.dot');

let currentSlide = 0;
const slideCount = slides.length;

let autoplayInterval = null;
let autoplayIsActive = false;

let userInteractionTimeout = null;

let touchStartX = 0;
let touchEndX = 0;
let isDragging = false;
let isTransitioning = false;

let startTransform = 0;
let currentTransform = 0;
let lastDragPosition = 0;
let dragVelocity = 0;
let lastDragTime = 0;

const AUTOPLAY_DELAY = 3000;
const INTERACTION_DELAY = 5500;
const VELOCITY_THRESHOLD = 0.5;

function updateDots(index) {
    let normalizedIndex = ((index % slideCount) + slideCount) % slideCount;
    dots.forEach((dot, i) => {
        dot.classList.toggle('active', i === normalizedIndex);
    });
}

function resetSlidePosition() {
    if (currentSlide < 0) {
        currentSlide = slideCount - 1;
        controller.style.transition = 'none';
        controller.style.transform = `translateX(-${(currentSlide + 1) * 100}%)`;
    } else if (currentSlide >= slideCount) {
        currentSlide = 0;
        controller.style.transition = 'none';
        controller.style.transform = `translateX(-${(currentSlide + 1) * 100}%)`;
    }
    // Force reflow to ensure the transition is applied
    controller.offsetHeight;
    controller.style.transition = 'transform 0.3s ease-out';
    updateDots(currentSlide);
}

function updateGallery(transition = true) {
    if (!transition) {
        controller.style.transition = 'none';
    } else {
        controller.style.transition = 'transform 0.3s ease-out';
    }

    controller.style.transform = `translateX(-${(currentSlide + 1) * 100}%)`;
    updateDots(currentSlide);

    if (transition) {
        isTransitioning = true;
        setTimeout(() => {
            isTransitioning = false;
            resetSlidePosition();
        }, 300);
    }
}

function startAutoplay() {
    stopAutoplay();
    autoplayInterval = setInterval(function () {
        if (!isDragging && !isTransitioning) {
            currentSlide = (currentSlide + 1) % slideCount;
            updateGallery();
        }
    }, AUTOPLAY_DELAY);
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
        if (!isDragging) startAutoplay();
    }, INTERACTION_DELAY);
}

function handleDragStart(event) {
    if (isTransitioning) return;
    isDragging = true;
    const clientX = event.type.includes('mouse') ? event.clientX : event.touches[0].clientX;
    startX = clientX;
    lastDragPosition = clientX;
    lastDragTime = Date.now();
    startTransform = (currentSlide + 1) * -100;
    controller.style.transition = 'none';
    controller.style.cursor = 'grabbing';
    handleUserInteraction();
}

function handleDrag(event) {
    if (!isDragging) return;

    event.preventDefault();
    const clientX = event.type.includes('mouse') ? event.clientX : event.touches[0].clientX;
    const currentTime = Date.now();
    const timeDiff = currentTime - lastDragTime;
            
    if (timeDiff > 0) {
        dragVelocity = (clientX - lastDragPosition) / timeDiff;
    }
            
    lastDragPosition = clientX;
    lastDragTime = currentTime;

    const diff = clientX - startX;
    const newTransform = startTransform + (diff / controller.offsetWidth * 100);
    currentTransform = newTransform;
            
    controller.style.transform = `translateX(${newTransform}%)`;
}

function handleDragEnd() {
    if (!isDragging) return;
    isDragging = false;
    controller.style.transition = 'transform 0.3s ease-out';
    controller.style.cursor = 'grab';

    const diff = currentTransform - startTransform;
    const absDiff = Math.abs(diff);
    const direction = Math.sign(diff);

    // Consider both drag distance and velocity for slide change
    if (absDiff > 20 || Math.abs(dragVelocity) > VELOCITY_THRESHOLD) {
        if (direction > 0) {
            currentSlide--;
        } else {
            currentSlide++;
        }
    }

    updateGallery();
    dragVelocity = 0;
}

controller.addEventListener('touchstart', handleDragStart, { passive: true });
controller.addEventListener('touchmove', handleDrag, { passive: false });
controller.addEventListener('touchend', handleDragEnd);
controller.addEventListener('touchcancel', handleDragEnd);

controller.addEventListener('mousedown', handleDragStart);
controller.addEventListener('mousemove', handleDrag);
controller.addEventListener('mouseup', handleDragEnd);
controller.addEventListener('mouseleave', handleDragEnd);

controller.addEventListener('dragstart', (e) => e.preventDefault());

// Handle transition end
controller.addEventListener('transitionend', () => {
    isTransitioning = false;
    resetSlidePosition();
});

dots.forEach((dot, index) => {
    dot.addEventListener('click', () => {
        if (!isTransitioning) {
            currentSlide = index;
            updateGallery();
            handleUserInteraction();
        }
    });
});

startAutoplay();