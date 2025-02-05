const navbarToggler = document.getElementById('navbarToggler');
const mobileMenu = document.getElementById('mobileMenu');
const dropdownButton = document.getElementById('dropdownButton');
const dropdownMenu = document.getElementById('dropdownMenu');

navbarToggler?.addEventListener('click', () => {
    mobileMenu.classList.toggle('hidden');
});

dropdownButton?.addEventListener('click', (e) => {
    e.preventDefault();
    dropdownMenu.classList.toggle('hidden');
});

document.addEventListener('click', (e) => {
    if (!dropdownButton?.contains(e.target)) {
        dropdownMenu?.classList.add('hidden');
    }
    if (!navbarToggler?.contains(e.target) && !mobileMenu?.contains(e.target)) {
        mobileMenu?.classList.add('hidden');
    }
});

window.addEventListener('resize', () => {
    if (window.innerWidth >= 768) {
        mobileMenu.classList.add('hidden');
    }
});