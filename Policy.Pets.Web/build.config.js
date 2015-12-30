module.exports = {
    dir: {
        build: 'build',
        compile: 'bin',
        assets: 'assets',
        vendor: 'vendor',
        app: 'app',
        lib: 'lib',
        src: 'src',
        config: 'config',
        images: 'images',
        fonts: 'fonts',
        webinf: 'WEB-INF'
    },
    
    src:{
        css:['src/**/*.css'],
        json:['src/**/*.json'],
        config: 'config/**/*',
        assets: 'src/assets/**/*',
        fonts: 'fonts/**/*.{ttf,woff,eof,svg}',
        js: ['src/**/*.js'],
        scripts: ['js/**/*.js'],
        ts : ['src/**/*.ts'],
        test : ['test/unit/**/*.ts'],
        tslibs : 'libs/**/*.ts',
        html: ['src/**/*.html'],
        images: ['images/*'],
        webinf: ['WEB-INF/*'],
        index: 'src/index.html',
        lib: {
            js: ['lib/**/*.js'],
            css: ['lib/**/*.css']
        }
    }
};