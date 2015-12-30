/**
 * Define dependencies
 */
var Q = require("q"),
    gulp = require('gulp'),
    gulpif = require('gulp-if'),
    gutil = require('gulp-util'),
    path = require('path') ,
    bowerFiles = require('gulp-bower-files'),
    minifycss = require('gulp-minify-css'),
    jshint = require('gulp-jshint'),
    jshintStylish = require('jshint-stylish-ex'),
    uglifyjs = require('gulp-uglifyjs'),
    uglify = require('gulp-uglify'),
    rename = require('gulp-rename'),
    clean = require('gulp-clean'),
    concat = require('gulp-concat'),
    livereload = require('gulp-livereload'),
    inject = require("gulp-inject"),
    autoprefixer = require('gulp-autoprefixer'),
    htmlmin = require('gulp-htmlmin'),
    runSequence = require('run-sequence'),
    mergestream = require('merge-stream'),
    gulpFilter = require('gulp-filter'),
    ts = require('gulp-type'),
    karma = require('gulp-karma')
    gulpProtractorAngular = require('gulp-angular-protractor');

/**
 * Load config files
 */
var pkg = require('./package.json');
var cfg = require('./build.config.js');

var destDir = path.join(cfg.dir.build, cfg.dir.app),
    assetDir = path.join(cfg.dir.build, cfg.dir.assets),
    imagesDir = path.join(cfg.dir.build, cfg.dir.images),
    fontsDir = path.join(cfg.dir.build, cfg.dir.fonts);

var sources = {
    css: {
        vendor: [path.join(assetDir, 'bootstrap.min.css'),
                 path.join(assetDir, '**', '*.css')],
        src: [path.join(assetDir, 'app.css')]
    },
    vendor: [path.join(destDir, 'jquery', '**', '*.js'),
             path.join(destDir, 'angular', '**', '*.js'),
             path.join(destDir, 'angular-route', '**', '*.js'),
             path.join(destDir, 'angular-mocks', '**', '*.js'),
             path.join(destDir, 'angular-resource', '**', '*.js'),
             path.join(destDir, 'angular-ui-router', '**', '*.js'),
             path.join(destDir, 'bootstrap', '**', '*.js')
    ],
    js: [
            path.join(destDir, 'services', '**', '*.js'),
            path.join(destDir, 'controllers', '**', '*.js'),
            path.join(destDir, 'configs', '**', '*.js'),
            path.join(destDir, 'app.js')]
};

// ----------------------------------------------------------------------------
// COMMON TASKS
// ----------------------------------------------------------------------------
/**
 * Clean build (bin) and compile (compile) directories
 */
gulp.task('clean', function () {
    return gulp.src([cfg.dir.build, cfg.dir.compile], { read: false })
        .pipe(clean());
});


// ----------------------------------------------------------------------------
// BASIC BUILD TASKS
// ----------------------------------------------------------------------------

/**
 * App css
 */
gulp.task('app:css:build', function () {
    var destDir = path.join(cfg.dir.build, cfg.dir.assets);

    var cssFilter = gulpFilter('**/*.css');
    var vendorCss = bowerFiles().pipe(cssFilter);
    var vendorFonts = bowerFiles().pipe(gulpFilter(['**/*.woff', '**/*.eot', '**/*.svg', '**/*.ttf', '**/*.woff2']));

    var appCss = gulp.src(cfg.src.assets);
    var bootstrapCss = [cfg.dir.vendor + "/bootstrap/dist/css/bootstrap.min.css"];

    gulp.src(bootstrapCss).pipe(gulp.dest(assetDir));

    return mergestream(vendorCss, appCss, vendorFonts)
          .pipe(gulp.dest(destDir));
});

gulp.task('app:css:bundle', function () {

    if (pkg.debug == true) return;

    logHighlight("Bundling css files");

    var css = sources.css.src.concat(sources.css.vendor);

    return gulp.src(css)
        .pipe(concat('app.' + pkg.version + (pkg.minifyCss == true ? '.min' : '') + '.css'))
        .pipe(gulpif(pkg.minifyCss == true, minifycss()))
        .pipe(gulp.dest(assetDir));
});

/**
 * JS
 */
gulp.task('app:js:build', function () {
    logHighlight("Copy js files");
    var destDir = path.join(cfg.dir.build, cfg.dir.app);
    var defDir = path.join(cfg.dir.build, 'definitions')
    var jsFilter = gulpFilter('**/*.js');

    var vendorJS = bowerFiles()
        .pipe(jsFilter);

    var src = cfg.src.ts;
    src.push(cfg.src.tslibs);
    src.push('!' + cfg.src.assets);
    
    var tsResult = gulp.src(src)
        .pipe(ts({
            declarationFiles: true,
            noExternalResolve: true
        }));

    tsResult.dts.pipe(gulp.dest(defDir));

    var scripts = gulp.src(cfg.src.scripts);

    return mergestream(vendorJS, tsResult.js, scripts)
          .pipe(gulp.dest(destDir));
});

gulp.task('app:js:bundle', function () {

    if (pkg.debug == true) return;

    logHighlight("Bundling js files");

    var src = sources.vendor.concat(sources.js);

    return gulp.src(src)
        .pipe(concat('app.' + pkg.version + (pkg.minifyJs == true ? '.min' : '') + '.js'))
        .pipe(gulpif(pkg.minifyJs == true, uglify()))
        .pipe(gulp.dest(destDir));
});


/**
 * Typescript: ts lint and compile
 */
gulp.task('tests:build', ['app:build'], function () {
    var destDir = path.join(cfg.dir.build ,'tests');
    logHighlight("Compiling Typescript test files to js files to dir: " + destDir);

    var src = cfg.src.test;
    src.push('build/definitions/**/*.ts');
    src.push(cfg.src.tslibs);
    src.push('!' + cfg.src.assets);
    var tsResult = gulp.src(src)
        .pipe(ts({
            declarationFiles: true,
            noExternalResolve: true,
            sortOutput:true
        }));

    //tsResult.dts.pipe(gulp.dest('release/definitions'));
    return tsResult.js
        .pipe(gulpif(pkg.debug == false, concat('tests.'+ pkg.version + '.js')))
        .pipe(gulp.dest(destDir));
});

/**
 * This task runs the test cases using karma.
 */
gulp.task('app:test',['tests:build'], function(done) {
    // Be sure to return the stream
    return gulp.src('./idontexist')
        .pipe(karma({
            configFile: 'test/test-unit-build.conf.js',
            action: 'run'
        }))
        .on('error', function(err) {
            // Make sure failed tests cause gulp to exit non-zero
            throw err;
        });
});

/**
 *  JSON
 */
gulp.task('app:json:build', function () {
    return gulp.src(cfg.src.json)
        .pipe(gulp.dest(cfg.dir.build));
});

// Images
gulp.task('app:images', function () {
    return gulp.src(cfg.src.images)
            .pipe(gulp.dest(imagesDir));
});

gulp.task('app:copyfonts', function () {
    var dest = path.join(cfg.dir.build, cfg.dir.assets, '/bootstrap/dist/fonts');
    return gulp.src(cfg.src.fonts)
            .pipe(gulp.dest(dest));
});



// Data
gulp.task('data', function () {
    return gulp.src([
            'app/scripts/**/*.csv',
            'app/scripts/**/*.json'])
        .pipe(gulp.dest('dist/scripts'))
        .pipe($.size());
});

/**
 * index.html: inject css and js files
 */
gulp.task('html:build', function () {
    var destDir = path.join(cfg.dir.build);
    var ignorePath = path.join(cfg.dir.build);
    
    gulp.src(cfg.src.webinf)
        .pipe(gulp.dest(path.join(cfg.dir.build, cfg.dir.webinf)));

    var src = sources.vendor.concat(sources.js);
    var css = sources.css.vendor.concat(sources.css.src);

    if (pkg.debug == false) {
        src = [path.join(cfg.dir.build, cfg.dir.app, 'app.' + pkg.version + (pkg.minifyJs == true ? '.min' : '') + '.js')];
        css = [path.join(cfg.dir.build, cfg.dir.assets, 'app.' + pkg.version + (pkg.minifyCss == true ? '.min' : '') + '.css')];
    }
  
    return gulp.src(cfg.src.html)
        .pipe(inject(gulp.src(src, {read: false}), {ignorePath: ignorePath, starttag: '<!-- inject:app:{{ext}} -->'}))
        .pipe(inject(gulp.src(css, {read: false}), {ignorePath: ignorePath}))
        .pipe(gulp.dest(destDir));
});


/**
 *
 */
gulp.task('app:build', function () {
    var deferred = Q.defer();

    runSequence(
        'clean',
        'app:css:build',
        'app:css:bundle',
        'app:js:build',
        'app:js:bundle',
        'app:json:build',
        'app:images',
        'app:copyfonts',
        'html:build',
        function () {
            deferred.resolve();
        });

    return deferred.promise;
});

gulp.task('unit', function (done) {
    // Be sure to return the stream
    return gulp.src('./idontexist')
        .pipe(karma({
            configFile: 'test/test-unit-src.conf.js',
            action: 'run'
        }))
        .on('error', function (err) {
            // Make sure failed tests cause gulp to exit non-zero
            throw err;
        });
});

// Setting up the test task 
gulp.task('e2e', function (callback) {
    return gulp.src(['./idontexist'])
            .pipe(gulpProtractorAngular({
                'configFile': 'test/protractor.conf.js',
                'debug': false,
                'autoStartStopServer': true
            }))
            .on('error', function (e) {
                console.log(e);
            })
            .on('end', callback);
});

// ----------------------------------------------------------------------------
// WATCH BUILD TASKS
// ----------------------------------------------------------------------------


gulp.task('watch', ['app:build'], function () {
    var server = livereload();

    // .css files
    gulp.watch('src/**/*.css', ['app:css:build']);
    // .js files
    gulp.watch('src/**/*.js', ['app:js:build']);
    // .html files
    gulp.watch('src/**/*.html', ['html:build']);

    var buildDir = path.join(cfg.dir.build, '**');
    gulp.watch(buildDir).on('change', function (file) {
        server.changed(file.path);
    });
});


// ----------------------------------------------------------------------------
// HELPER FUNCTIONS
// ----------------------------------------------------------------------------

/**
 * Highlight debug messages in log
 * @param message
 */
function logHighlight(message) {
    gutil.log(gutil.colors.black.bgWhite(message));
};