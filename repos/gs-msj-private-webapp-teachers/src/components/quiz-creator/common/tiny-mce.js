/*
*   Create By: Mario Cancel
*/
import React from 'react';
import { Editor } from '@tinymce/tinymce-react';
import { connect } from 'react-redux';
import { selectPremise } from '../../../redux/actions';
import LanguageManager from '../../../custom-classes/language-manager';
import Settings from "../../../custom-classes/settings";

const TextEditor = ({onChange, wasKeyDownReg, premiseId, value, disabled,  selectPremise, placeholder, showMenuBar, onKeyDownReg}) =>  {


    if(typeof showMenuBar === "undefined" || showMenuBar === null)
    {
        showMenuBar = true;
    }


    return (



        <Editor
            apiKey={Settings.getTinyMCELicense}
            value={value}
            disabled={disabled}
            onFocus={() => selectPremise(premiseId)}
            init={{
                plugins: 'link image code media table lists',
                toolbar: showMenuBar ?  Settings.getTextEditorConfigs() : '',
                language: LanguageManager.getLanguage() !== "en"? 'es':'en',
                // WirisButton
                setup: function(editor){
                    editor.ui.registry.addButton('addColonsButton', {
                        icon: 'quote',
                        tooltip: 'Insert colons',
                        onAction: () => editor.insertContent("“”")
                    });


                },
                placeholder: placeholder,
                menubar: 'edit format table',
                menu: {
                    edit: {title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall'},
                    format: {title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat'},
                    table: {title: 'Table', items: 'inserttable tableprops deletetable | cell row column'},
                },
                paste_data_images: true,
                external_plugins: {
                    'tiny_mce_wiris': Settings.getTinyMCEScriptURL(),
                },
                formats: {
                    underline: {inline: 'u'}
                },
                height : !showMenuBar ? "300" : "280",
                outputFormat:'text',
                audio_template_callback: function(d) {
                    return '<audio controls>\n<source src="' + d.source1 + '"' + (d.source1mime ? ' type="' + d.source1mime + '"' : '') + ' />\n</audio>';
                }
            }}
            onKeyDown={()=>{
                onKeyDownReg(true)
            }}
            onEditorChange = {
                (content, event) => {
                    if(wasKeyDownReg) {
                        onChange(premiseId, content)
                    }
                }
            }
        />
    );
};

const mapStateToProps = state => {
    return {
        state
    }
};

const actionCreators = {
    selectPremise
};

export default connect(mapStateToProps, actionCreators)(TextEditor);