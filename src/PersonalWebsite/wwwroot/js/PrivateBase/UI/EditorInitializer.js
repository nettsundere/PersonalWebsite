import * as monaco from 'monaco-editor/esm/vs/editor/editor.api';

export default class EditorInitializer {
    static Initialize(editorSelector)
    {
        document.querySelectorAll(editorSelector).forEach((item) => {
            const fieldValueAttribute = "value";
            const field = item.nextElementSibling;
            
            const editor = monaco.editor.create(item, {
                value: field.getAttribute(fieldValueAttribute),
                language: "html",
                theme: "vs-dark",
                contextmenu: true,
                quickSuggestions: true,
                automaticLayout: true
            });

            editor.onDidChangeModelContent(() => {
                field.setAttribute(fieldValueAttribute, editor.getValue());
            });
        });
    }
}
