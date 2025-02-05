document.addEventListener("DOMContentLoaded", function () {
    const adminDropdownButton = document.getElementById("adminDropdownButton");
    const adminDropdownMenu = document.getElementById("adminDropdownMenu");
    const adminLogout = document.getElementById("adminLogout");

    if (adminDropdownButton && adminDropdownMenu) {
        // Toggle Admin Dropdown
        adminDropdownButton.addEventListener("click", (e) => {
            e.preventDefault();
            adminDropdownMenu.classList.toggle("hidden");
        });

        // Close Dropdown when Clicking Outside (Except Logout)
        document.addEventListener("click", (e) => {
            if (!adminDropdownButton.contains(e.target) && !adminDropdownMenu.contains(e.target)) {
                adminDropdownMenu.classList.add("hidden");
            }
        });

        // Ensure Logout Works Properly
        adminLogout.addEventListener("click", function () {
            adminDropdownMenu.classList.add("hidden"); // Close dropdown on logout click
        });
    }
});
