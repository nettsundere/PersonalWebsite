/// <binding BeforeBuild='inject' Clean='clean' />

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    project = require("./project.json");

var paths = {
    webroot: "wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/Build/site.min.js";
paths.concatCssDest = paths.webroot + "css/Build/site.min.css";
paths.vendorJsDest = paths.webroot + "js/Build/vendor.js";
paths.vendorCssDest = paths.webroot + "css/Build/vendor.css";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

var series = require("stream-series");
var inject = require("gulp-inject");

gulp.task("inject", ["min"], function () {
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
        series(
            series(vendorCss, ownCss),
            series(vendorJs, ownJs)
        ),
        { ignorePath: "wwwroot" }))
        .pipe(gulp.dest(sourceDir));
});