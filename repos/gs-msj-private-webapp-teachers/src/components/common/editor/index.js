import React, { useMemo, useState, useRef, forwardRef, useImperativeHandle, useEffect } from 'react';
import katex from "katex";
import ReactQuill from "../react-quill";
import "quill/dist/quill.snow.css";
import { MdFunctions } from "react-icons/md";
import CircularProgress from '@material-ui/core/CircularProgress';
import ModalFormula from "../modal-formula";
import { toBlob } from "html-to-image";
import AudioBlot from "./embed/audio";
import VideoBlot from "./embed/video";
import Settings from '../../../custom-classes/settings';
import { GrUndo, GrRedo } from "react-icons/gr";
import './editor.css';
import QuillTableBetter from 'quill-table-better';

// Assigns the KaTeX library to the global window object.
// This makes KaTeX available throughout the application, 
// allowing for mathematical expressions to be rendered.
window.katex = katex;

// Registers a custom module with Quill editor.
ReactQuill.Quill.register({
  "formats/audio": AudioBlot,
  "formats/video": VideoBlot,
  'modules/table-better': QuillTableBetter
}, true);

/**
 * Editor component using ReactQuill for rich text editing.
 * @component
 * @param {Object} props - Component props
 * @param {string} props.defaultValue - Initial content of the editor
 * @param {string} props.placeholder - Placeholder text for the editor
 * @param {boolean} props.disabled - If true, the editor is read-only
 * @param {function} props.onChange - Callback function to handle content changes
 * @param {function} props.onFocus - Callback function to handle focus event
 * @param {Object} props.toolbar - Configuration options for the toolbar
 * @param {React.Ref} ref - Forwarded ref to access editor methods
 * @returns {JSX.Element} The rendered Editor component
 * @example
 * <Editor
 *   defaultValue="Hello, world!"
 *   placeholder="Start typing..."
 *   disabled={false}
 *   onChange={(content) => console.log(content)}
 *   onFocus={() => console.log('Editor focused')}
 *   toolbar={{ showToolBar: true, undo: true, redo: true }}
 *   ref={editorRef}
 * />
 * @method insertText - Inserts text at the current cursor position
 * @method insertMedia - Inserts media (image, video, etc.) at the current cursor position
 */
const Editor = forwardRef(({ defaultValue, placeholder, disabled, onChange, onFocus, toolbar }, ref) => {
  const fieldRef = useRef();
  const editorRef = useRef();
  const [loading, setLoading] = useState(false);
  const randomId = useMemo(() => Math.random().toString(36).substring(7), []);
  const [content, setContent] = useState(defaultValue);
  const [open, setOpen] = useState(false);
  const [lastCursorPosition, setLastCursorPosition] = useState(0);

  const options = {
    ...Settings.getDefaultQuillConfigs(),
    ...toolbar
  };

  /**
   * Retrieves the editor instance and the current cursor position.
   * @returns {Object} An object containing the editor instance and the cursor position.
   */
  function getEditor() {
    const editor = editorRef.current?.getEditor();
    if (!editor)
      return { editor: null, cursor: 0 };

    const range = editor.getSelection();
    const cursor = range ? range.index : lastCursorPosition;

    return {
      editor,
      cursor
    };
  }

  /**
   * Inserts the given text at the current cursor position in the editor.
   * @param {string} value - The text to be inserted into the editor.
   */
  const insertText = (value) => {
    const { editor, cursor } = getEditor();
    editor.insertText(cursor, value);
  }

  /**
   * Inserts media into the editor at the current cursor position.
   * @param {string} type - The type of media to insert (e.g., 'image', 'video').
   * @param {string} value - The value or source of the media to insert.
   */
  const insertMedia = (type, value) => {
    const { editor, cursor } = getEditor();
    editor.insertEmbed(cursor, type, value);
   // After inserting the media, a new line is added and the cursor is moved to the position after the new line.
    editor.insertText(cursor + 1, '\n');
    editor.setSelection(cursor + 2);
  }

  useImperativeHandle(ref, () => ({
    insertText,
    insertMedia
  }));

  /**
   * Checks if the given options object has any truthy values.
   * @param {Object} options - The options object to check.
   * @returns {boolean} - Returns true if any value in the options object is truthy, otherwise false.
   */
  function hasOptions(options) {
    return Object.values(options).some(Boolean);
  }

  /**
   * Handles the blur event for the editor.
   * Retrieves the current cursor position from the editor and sets it as the last cursor position.
   */
  function handleBlur() {
    const { cursor } = getEditor();
    setLastCursorPosition(cursor);
  }

  /**
   * Handles changes in the editor and updates the content.
   * @param {string} changes - The new content from the editor.
   */
  function handleEditorChange(changes) {
    setContent(changes);
    onChange(changes);
  }

  /**
   * Handles the undo action for the editor.
   */
  function handleUndo() {
    const { editor } = getEditor();
    editor.history.undo();
  }

  /**
   * Redoes the last undone action in the editor's history.
   */
  function handleRedo() {
    const { editor } = getEditor();
    editor.history.redo();
  }

  /**
   * Checks if a history button is disabled based on the editor's history stack.
   * @param {string} button - The name of the history button to check (e.g., 'undo', 'redo').
   * @returns {boolean} - Returns true if the history stack for the specified button is empty, otherwise false.
   */
  function isHistoryButtonDisabled(button) {
    const { editor } = getEditor();
    return editor?.history.stack[button].length <= 0;
  }

  /**
   * Handles the event when a formula changes.
   * Converts the formula to a blob, reads it as a data URL,
   * and inserts it as an image media.
   * @returns {Promise<void>} A promise that resolves when the operation is complete.
   */
  async function handleFormulaChanges() {
    if (fieldRef.current === null)
      return;

    setLoading(true);
    try {
      const blob = await toBlob(fieldRef.current, {
        cacheBust: true,
        style: { height: '90%', padding: '0px', margin: '0px' }
      });
      const reader = new FileReader();
      reader.onloadend = function () {
        insertMedia('image', reader.result);
      };

      reader.readAsDataURL(blob);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    if (!defaultValue || defaultValue === content)
      return;
    setContent(defaultValue)
  }, [defaultValue]);

  return (
    <>
      <div id={`toolbar-${randomId}`} className={options.showToolBar ? 'ql-toolbar' : 'ql-hidden'}>

        {(options.undo || options.redo) && (
          <span className="ql-formats formats-history">
            {options.undo && <button disabled={isHistoryButtonDisabled('undo')} onClick={() => handleUndo()}>
              <GrUndo />
            </button>}
            {options.redo && <button disabled={isHistoryButtonDisabled('redo')} onClick={() => handleRedo()}>
              <GrRedo />
            </button>}
          </span>
        )}

        {(options.size || options.header) && (
          <span className="ql-formats">
            {options.size && <select className="ql-size"></select>}
            {options.header && <select className="ql-header"></select>}
          </span>
        )}

        {hasOptions(options.format) && (
          <span className="ql-formats">
            {options.format.bold && <button className="ql-bold"></button>}
            {options.format.italic && <button className="ql-italic"></button>}
            {options.format.underline && <button className="ql-underline"></button>}
            {options.format.strike && <button className="ql-strike"></button>}
            {options.format.color && <select className="ql-color"></select>}
          </span>
        )}

        {hasOptions(options.list) && (
          <span className="ql-formats">
            {options.list.ordered && <button className="ql-list" value="ordered"></button>}
            {options.list.bullet && <button className="ql-list" value="bullet"></button>}
            {options.list.indent.remove && <button className="ql-indent" value="-1"></button>}
            {options.list.indent.add && <button className="ql-indent" value="+1"></button>}
          </span>
        )}

        {hasOptions(options.align) && (
          <span className="ql-formats">
            {options.align.left && <button className="ql-align"></button>}
            {options.align.center && <button className="ql-align" value="center"></button>}
            {options.align.right && <button className="ql-align" value="right"></button>}
            {options.align.justify && <button className="ql-align" value="justify"></button>}
          </span>
        )}

        {hasOptions(options.media) && (
          <span className="ql-formats">
            {options.media.video && <button className="ql-video"></button>}
            {options.media.image && <button className="ql-image"></button>}
          </span>
        )}

        {(options.blockquote || options.code) && (
          <span class="ql-formats">
            {options.blockquote && <button class="ql-blockquote"></button>}
            {options.code && <button class="ql-code-block"></button>}
          </span>
        )}

        {options.table && (
          <span class="ql-formats">
            <button class="ql-table-better"></button>
          </span>
        )}

        {options.formula && (
          <span className="ql-formats">
            <button onClick={(event) => {
              event.preventDefault();
              setOpen(true);
            }}>
              <MdFunctions />
            </button>
          </span>
        )}
      </div>
      <div style={{ position: 'relative' }}>
        {loading ?
          <CircularProgress
            style={{
              position: 'absolute',
              right: '5px',
              top: '5px',
              zIndex: 1,
              width: '10px',
              height: '10px'
            }} />
          : null}
        <ReactQuill
          ref={editorRef}
          theme="snow"
          value={content}
          readOnly={disabled}
          placeholder={placeholder}
          modules={{
            toolbar: `#toolbar-${randomId}`,
            table: true,
            'table-better': {
              language: 'en_US',
              menus: ['column', 'row', 'merge', 'table', 'cell', 'wrap', 'copy', 'delete'],
              toolbarTable: true
            },
            keyboard: {
              bindings: QuillTableBetter.keyboardBindings
            }
          }}
          onChange={handleEditorChange}
          onChangeSelection={handleBlur}
          onFocus={onFocus}
        />
      </div>
      <ModalFormula
        isOpen={open}
        onClose={() => setOpen(false)}
        fieldRef={fieldRef}
        onChange={handleFormulaChanges}
      />
    </>
  );
});

Editor.defaultProps = {
  defaultValue: "",
  placeholder: "",
  disabled: false,
  onChange: () => { },
  onFocus: () => { },
  toolbar: {}
}

export default Editor;