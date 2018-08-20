import Editor from '@ckeditor/ckeditor5-build-classic';

export default class EditorInitializer {
    static Initialize(editorSelector)
    {
        document.querySelectorAll(editorSelector).forEach((item) => {
            Editor.create(item);  
        })
    }
}


