import React, {useState, useRef} from 'react'
import ReactQuill from 'react-quill';
import { Translate } from 'react-localize-redux';
import Settings from '../../../custom-classes/settings';
import { Alert } from 'reactstrap';

// Component to create a new post
const InputCreatePost = ({ postContent, postContentChange, publishPost, hidePost }) => {
    const quillRef = useRef();
    const [showError, setShowError] = useState(false);

    const handleContentChange = (content) => {
        if (!quillRef.current) return;
        postContentChange(content);

        const contentLength = content.length;
        if (contentLength > Settings.FORUM_MAX_CHARACTERS) {
            setShowError(true);
        } else {
            setShowError(false);
        }
    };

    const checkCharacterCount = (event) => {
        if (!quillRef.current) return; 

        const plainText = quillRef.current.getEditor().getText().trim();
        if (plainText.length > Settings.FORUM_MAX_CHARACTERS && event.key !== 'Backspace') {
            event.preventDefault();
        }
    };

    return (
        <div className="foroHeader--formGroup">
            <ReactQuill
                ref={quillRef}
                modules={Settings.FORUM_MODULES}
                theme="snow"
                value={postContent}
                onChange={handleContentChange}
                onKeyDown={checkCharacterCount}
            />
            <div className="message-content-buttons-mail">
                {showError ? 
                    <Alert color="info">
                        <Translate id="forum.error-length-post" options={{ renderInnerHtml: true }} />
                        {' '} {Settings.FORUM_MAX_CHARACTERS}
                    </Alert> 
                : null}
                <button 
                    disabled={showError} 
                    type="submit" 
                    className="btn-reply-submit" 
                    onClick={() => publishPost(quillRef.current.getEditor().getText().trim())}
                >
                    <Translate id="forum.reply" options={{ renderInnerHtml: true }} />
                </button>
                <button type="submit" className="btn-reply-cancel" onClick={hidePost}>
                    <Translate id="forum.cancel" options={{ renderInnerHtml: true }} />
                </button>
            </div>
        </div>
    );
};

export default InputCreatePost