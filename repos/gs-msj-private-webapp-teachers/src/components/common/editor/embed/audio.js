import ReactQuill from "../../react-quill";

// Import the Embed class from Quill
// Use an inline embed ('blots/embed') for custom audio blots to avoid crashes
// when inserting them into quill-table-better. Block embeds ('blots/block/embed')
// were causing the editor to break
const Embed = ReactQuill.Quill.import('blots/embed');

/**
 * AudioBlot class to handle audio embedding in Quill editor.
 * Extends the Embed class from Quill.
 */
class AudioBlot extends Embed {
  /**
   * Creates an audio node with the given value (URL).
   * @param {string} value - The URL of the audio file.
   * @returns {HTMLElement} The created audio node.
   */
  static create(value) {
    let node = super.create();
    node.setAttribute('controls', 'controls');
    const src = value && typeof value === 'object' && value.url ? value.url : value;
    node.setAttribute('src', src);
    return node;
  }

  /**
   * Retrieves the value (URL) from the audio node.
   * @param {HTMLElement} node - The audio node.
   * @returns {Object} An object containing the URL of the audio file.
   */
  static value(node) {
    return {
      url: node.getAttribute('src')
    };
  }
}

// Define the blot name and tag name for the AudioBlot
AudioBlot.blotName = 'audio';
AudioBlot.tagName = 'audio';

export default AudioBlot;
