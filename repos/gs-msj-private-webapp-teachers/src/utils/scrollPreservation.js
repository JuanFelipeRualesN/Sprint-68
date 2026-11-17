// Function to preserve scroll position when re-rendering
export const scrollPreservation = async (operation) => {
    const scrollPosition = window.scrollY;
    try {
        await operation();
    } finally {
        setTimeout(() => {
            window.scrollTo({
                top: scrollPosition,
                behavior: 'instant'
            });
        }, 0);
    }
};