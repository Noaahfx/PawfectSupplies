/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './PawfectSupplies/MasterPages/**/*.master', // Include MasterPages
    './PawfectSupplies/Pages/**/*.aspx',        // Include ASPX pages
    './PawfectSupplies/Scripts/**/*.js',        // Include custom JS files
  ],
  theme: {
    extend: {
      animation: {
        'fade-in': 'fadeIn 1.5s ease-in-out',
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: 0 },
          '100%': { opacity: 1 },
        },
      },
      colors: {
        teal: {
          50: '#f0fdfa',
          100: '#ccfbf1',
          200: '#99f6e4',
          300: '#5eead4',
          400: '#2dd4bf',
          500: '#14b8a6',
          600: '#0d9488',
          700: '#0f766e',
          800: '#115e59',
          900: '#134e4a',
        },
      },
    },
  },
  plugins: [],
};
