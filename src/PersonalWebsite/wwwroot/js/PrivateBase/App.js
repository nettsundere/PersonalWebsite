// CSS
import "../../css/PrivateBase/site.css";
import "bootstrap/dist/css/bootstrap.min.css";

// JavaScript
import "jquery"
import "bootstrap"

import EditorInitializer from "./UI/EditorInitializer"

$().ready(() => {
    EditorInitializer.Initialize("[data-role='content-editor']");
});
