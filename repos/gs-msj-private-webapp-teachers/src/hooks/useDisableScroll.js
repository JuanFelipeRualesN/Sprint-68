import { useEffect } from 'react';

const useDisableScroll = (isVisible) => {
  useEffect(() => {
    const handleWindowWheel = (event) => {
      if (isVisible){
        event.preventDefault();
      }
    };

    window.addEventListener('wheel', handleWindowWheel, { passive: false });

    return () => {
      window.removeEventListener('wheel', handleWindowWheel);
    };
  }, [isVisible]);
};

export default useDisableScroll;
