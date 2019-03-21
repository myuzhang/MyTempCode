const TestInfo = require('./TestInformation')

var test;

before(() => {
  test = new TestInfo();
})

beforeEach (() => {
  console.log('global before each');
})

afterEach(function() {
  console.log('global after each');
  test.result = {title: this.currentTest.title, state: this.currentTest.state};
})

after(() => {
  test.results.forEach(r => {
    console.log(r.title, r.state);
  })
})
