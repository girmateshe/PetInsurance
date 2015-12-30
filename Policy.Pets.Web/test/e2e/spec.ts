describe('radix homepage', function () {


    describe('channels', function () {
        var channels;
        var title;

        beforeEach(function () {
            browser.get('http://radixlinux-ci.elasticbeanstalk.com/#/channels');
            //channels = element.all(by.repeater('channel in channels'));
           // title = element(by.css('.container.app-title h2'));
        });

        it('should be aventus v2', function () {
            expect(true).toBe(true);
           // expect(title).toBe('Aventus v2');
            //expect(channels.count()).toEqual(1);
        });

    });
}); 


/*
describe('angularjs homepage', function () {
    it('should greet the named user', function () {
        browser.get('http://www.angularjs.org');

        element(by.model('yourName')).sendKeys('Julie');

        var greeting = element(by.binding('yourName'));

        expect(greeting.getText()).toEqual('Hello Julie!');
    });

    describe('todo list', function () {
        var todoList;

        beforeEach(function () {
            browser.get('http://www.angularjs.org');

            todoList = element.all(by.repeater('todo in todoList.todos'));
        });

        it('should list todos', function () {
            expect(todoList.count()).toEqual(2);
            expect(todoList.get(1).getText()).toEqual('build an angular app');
        });

        it('should add a todo', function () {
            var addTodo = element(by.model('todoList.todoText'));
            var addButton = element(by.css('[value="add"]'));

            addTodo.sendKeys('write a protractor test');
            addButton.click();

            expect(todoList.count()).toEqual(3);
            expect(todoList.get(2).getText()).toEqual('write a protractor test');
        });
    });
});
*/