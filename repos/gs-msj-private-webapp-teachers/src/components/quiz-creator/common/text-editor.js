/*
*   Create By: Mario Cancel
*/
import React from 'react';
import CKEditor from '@ckeditor/ckeditor5-react';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';

const TextEditor = ({onChange, premiseId, value, disabled}) =>  {

    return (
        <CKEditor
            editor={ClassicEditor}
            data={value}
            disabled={disabled}
            onChange={ ( event, editor ) => {
                onChange(premiseId, editor.getData())
            } }
        />
    );
};

export default TextEditor;