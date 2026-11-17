import ReactQuill from "../../react-quill";

// Import the Embed class from Quill
// Use an inline embed ('blots/embed') for custom video blots to avoid crashes
// when inserting them into quill-table-better. Block embeds ('blots/block/embed')
// were causing the editor to break
const Embed = ReactQuill.Quill.import('blots/embed');

/**
 * VideoBlot class to handle video embedding in Quill editor.
 * Extends the Embed class from Quill.
 */
class VideoBlot extends Embed {
  /**
   * Creates a video node with the given value (URL).
   * @param {string} value - The URL of the video file.
   * @returns {HTMLElement} The created video node.
   */
  static create(value) {
    let node = super.create();
    node.setAttribute('controls', 'controls');
    const src = value && typeof value === 'object' && value.url ? value.url : value;
    node.setAttribute('src', src);
    return node;
  }

  /**
   * Retrieves the value (URL) from the video node.
   * @param {HTMLElement} node - The video node.
   * @returns {Object} An object containing the URL of the video file.
   */
  static value(node) {
    return {
      url: node.getAttribute('src')
    };
  }
}

// Define the blot name and tag name for the VideoBlot
VideoBlot.blotName = 'video';
VideoBlot.tagName = 'video';

export default VideoBlot;
