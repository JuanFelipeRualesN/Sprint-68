import { useEffect } from 'react';
let currentMedia = null;
export const usePreventMediaOverlapping = (containerRef, section) => {
  useEffect(() => {
    if (!containerRef.current) return;
    // If there's a media playing, pause it when another media starts playing
    const handleMediaPlay = (mediaElement) => {
      if (currentMedia && currentMedia !== mediaElement) {
        currentMedia.pause();
      }
      currentMedia = mediaElement;
    };
    const setupMediaHandlers = (container) => {
      // Get all the audio and video elements of the container
      const audioElements = container.getElementsByTagName('audio');
     const videoElements = container.getElementsByTagName('video');
      const handlers = new Map();
      // Loop through all the audio elements
      Array.from(audioElements).forEach(audio => {
        const handler = () => handleMediaPlay(audio);
        handlers.set(audio, handler);
        audio.addEventListener('play', handler);
      });
      // Loop through all the video elements
      Array.from(videoElements).forEach(video => {
        const handler = () => handleMediaPlay(video);
        handlers.set(video, handler);
        video.addEventListener('play', handler);
      });
      return handlers;
    };

    let handlers = setupMediaHandlers(containerRef.current);
    
    // Config to track new media when user is creating a lesson, quiz etc
    const callback = (mutationsList, observer) => {
      for (const mutation of mutationsList) {
        if (mutation.type === 'childList') {
          // Check if the added node is an audio or a video
          const hasNewMedia = Array.from(mutation.addedNodes).some(node => {
            if (node.nodeName === 'AUDIO' || node.nodeName === 'VIDEO') {
              return true;
            }
            
            // Search for media if the added node is a container
            if (node.nodeType === Node.ELEMENT_NODE) {
              const audioElements = node.getElementsByTagName('audio');
              const videoElements = node.getElementsByTagName('video');
              return audioElements.length > 0 || videoElements.length > 0;
            }
            return false;
          });

          if (hasNewMedia) {
            handlers.forEach((handler, media) => {
              media.removeEventListener('play', handler);
            });
            handlers = setupMediaHandlers(containerRef.current);
          }
        }
      }
    };

    const config = { childList: true, subtree: true };
    const containerObserver = new MutationObserver(callback)
    containerObserver.observe(containerRef.current, config)

    // Remove the event listener and pause the media when the component unmounts
    return () => {
      containerObserver.disconnect();
      handlers.forEach((handler, media) => {
        media.removeEventListener('play', handler);
      });
      if (currentMedia) {
        currentMedia.pause();
        currentMedia = null;
      }
    };
  }, [containerRef, section]);
};