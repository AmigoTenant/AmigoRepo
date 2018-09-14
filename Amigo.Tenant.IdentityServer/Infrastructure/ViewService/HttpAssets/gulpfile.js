var gulp = require('gulp');
var concat = require('gulp-concat');
var cleanCSS = require('gulp-clean-css');
var uglify = require('gulp-uglify');
var clean = require('gulp-clean');

var paths = {
    scripts: [
        'libs/angular/angular.1.3.15.js',
        'libs/jquery/jquery-1.11.0.js',
        'libs/tether/js/tether.min.js',
        'libs/bootstrap/js/bootstrap.js',
        'libs/encoder/encoder.js',
        'app/crypto.js',
        'app/app.js'
    ],
    css: [
        'app/app.css',
        'libs/angular/angular-csp.css',
        'libs/bootstrap/css/bootstrap.css',
        'libs/bootstrap/css/bootstrap-theme.css'
    ]    
};

gulp.task('clean', function () {
    return gulp.src([
            'styles.min.css',
            'scripts.2.5.0.js'
        ])
        .pipe(clean({ force: true }));      
});

gulp.task('scripts', function () {    
    return gulp.src(paths.scripts)        
        .pipe(concat('scripts.2.5.0.js'))
        .pipe(gulp.dest("./"));
});

gulp.task('css', function () {
    return gulp.src(paths.css)
        .pipe(concat('styles.min.css'))
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(gulp.dest('./'));
});

gulp.task('watch', function () {
    gulp.watch(paths.scripts, ['scripts']);
    gulp.watch(paths.css, ['css']);
});

gulp.task('default', ['clean','scripts','css']);