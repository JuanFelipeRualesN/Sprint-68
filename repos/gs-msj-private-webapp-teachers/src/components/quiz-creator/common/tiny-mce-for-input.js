/*
*   Created By: Mario Cancel,  based on Luis Diaz TinyMCE script.
*/
import React from 'react';
import { Editor } from '@tinymce/tinymce-react';
import { withLocalize } from 'react-localize-redux';
import LanguageManager from '../../../custom-classes/language-manager';
import Settings from "../../../custom-classes/settings";

const TextEditorForInput = ({onAnswerChange, wasKeyDownAnswers, value, index, premiseId, disabled, selectAnswer, languages, onKeyDownAnswers}) =>  {

    return (
        <Editor
            apiKey={Settings.getTinyMCELicense}
            value={value}
            disabled={disabled}
            onFocusIn={() => selectAnswer(premiseId, index)}
            init={{
                plugins: 'link image code media table lists',
                toolbar:  Settings.getTextEditorConfigs(),
                language: LanguageManager.getLanguage() !== "en"? 'es':'en',
                // WirisButton
                setup: function(editor){
                    editor.ui.registry.addButton('addColonsButton', {
                        icon: 'quote',
                        tooltip: 'Insert colons',
                        onAction: () => editor.insertContent("“”")
                    });
                },
                placeholder: `${LanguageManager.getTranslation("quiz.enter-option", languages)} ${index + 1}`,
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
                templates: [
                    {title: 'Some title 1', description: 'Some desc 1', content: 'My content'},
                    {title: 'Some title 2', description: 'Some desc 2', url: 'development.html'}
                ],
                outputFormat:'text',
                audio_template_callback: function(d) {
                    return '<audio controls>\n<source src="' + d.source1 + '"' + (d.source1mime ? ' type="' + d.source1mime + '"' : '') + ' />\n</audio>';
                }
            }}
            onKeyDown={()=>{
                onKeyDownAnswers(true)
            }}
            onEditorChange={ ( content, event ) => {
                if(wasKeyDownAnswers) {
                    onAnswerChange(content, index)
                }
            }
            }
        />
    );
};

export default withLocalize(TextEditorForInput);

