/// <binding BeforeBuild='inject' />

var gulp = require("gulp"),
    del = require("del"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    project = require("./project.json");

var paths = {
    webroot: "wwwroot/"
};

paths.js = paths.webroot + "js/Base/*.js";

paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/Base/*.css";

paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/Build/site.min.js";

paths.concatCssDest = paths.webroot + "css/Build/site.min.css";

paths.vendorJsDest = paths.webroot + "js/Build/vendor.js";
paths.vendorCssDest = paths.webroot + "css/Build/vendor.css";

// Private website area
paths.private = {};
paths.private.js = paths.webroot + "js/PrivateBase/*.js";
paths.private.concatJsDest = paths.webroot + "js/Build/site.private.min.js";
paths.private.vendorJsDest = paths.webroot + "js/Build/vendor.private.js";

paths.private.css = paths.webroot + "css/PrivateBase/*.css";
paths.private.concatCssDest = paths.webroot + "css/Build/site.private.min.css";
paths.private.vendorCssDest = paths.webroot + "css/Build/vendor.private.css";

gulp.task("clean:js", function () {
    del([
        paths.concatJsDest,
        paths.vendorJsDest,
        paths.private.concatJsDest,
        paths.private.vendorJsDest
    ]);
});

gulp.task("clean:css", function () {
    del([
        paths.concatCssDest,
        paths.vendorCssDest,
        paths.private.concatCssDest,
        paths.private.vendorCssDest
    ]);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:private:js", function () {
    gulp.src([paths.private.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.private.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:js", function () {
    gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:private:css", function () {
    gulp.src([paths.private.css, "!" + paths.minCss])
        .pipe(concat(paths.private.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:private:js", "min:css", "min:private:css"]);

var series = require("stream-series");
var inject = require("gulp-inject");

gulp.task("inject", ["inject:public", "inject:private"]);

gulp.task("inject:private", ["clean", "min"], function () {
    // Private site layout
    var privateSourceDir = "./Areas/Private/Views/Shared/";
    var privateSourcePath = privateSourceDir + "_Layout.cshtml";
    var privateSource = gulp.src(privateSourcePath);

    var vendorCss = gulp.src([
        // Bootstrap
        "./wwwroot/lib/bootstrap/dist/css/bootstrap.min.css",
    ]).pipe(concat(paths.private.vendorCssDest)).pipe(gulp.dest("."));

    var ownJs = gulp.src([
        paths.private.concatJsDest
    ], { read: false });

    var ownCss = gulp.src([
    paths.private.concatCssDest
    ], { read: false });

    var vendorJs = gulp.src([
        // jQuery
        "./wwwroot/lib/jquery/dist/jquery.min.js",
        "./wwwroot/lib/jquery-validation/jquery.validate.js",
        "./wwwroot/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",

        // Bootstrap
        "./wwwroot/lib/bootstrap/dist/js/bootstrap.min.js",

        // CKEditor
        "./wwwroot/lib/ckeditor/ckeditor.js"
    ]).pipe(concat(paths.private.vendorJsDest)).pipe(gulp.dest("."));

    return privateSource.pipe(inject(series(vendorJs, ownJs, vendorCss, ownCss), { ignorePath: "wwwroot" }))
                        .pipe(gulp.dest(privateSourceDir));
});

gulp.task("inject:public", ["clean", "min"], function () {
    // Site layout
    var sourceDir = "./Views/Shared/";
    var sourcePath = sourceDir + "_Layout.cshtml";
    var source = gulp.src(sourcePath);

    var ownCss = gulp.src([
        paths.concatCssDest
    ], { read: false });

    var ownJs = gulp.src([
        paths.concatJsDest
    ], { read: false });

    var vendorJs = gulp.src([
        // jQuery
        "./wwwroot/lib/jquery/dist/jquery.min.js",
        "./wwwroot/lib/jquery-validation/jquery.validate.js",
        "./wwwroot/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",

        // Bootstrap
        "./wwwroot/lib/bootstrap/dist/js/bootstrap.min.js"
    ]).pipe(concat(paths.vendorJsDest)).pipe(gulp.dest("."));

    var vendorCss = gulp.src([
        // Bootstrap
        "./wwwroot/lib/bootstrap/dist/css/bootstrap.min.css",
    ]).pipe(concat(paths.vendorCssDest)).pipe(gulp.dest("."));

    return source.pipe(inject(
        series(vendorCss, ownCss, vendorJs, ownJs),
        { ignorePath: "wwwroot" }))
        .pipe(gulp.dest(sourceDir));
});
