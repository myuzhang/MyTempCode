const assert = require('chai').assert;
const expect = require('chai').expect;
const should = require('chai').should;

describe('test2', () => {

    before(() => {
        console.log('2-block1 before');
      })

    beforeEach(() => {
        console.log('2-block1 before each');
    });

    describe('test22', () => {

        before(() => {
            console.log('2-block2 before');
          })
    
        beforeEach(() => {
            console.log('2-block2 before each');
        });

        it('return -1', () => {
            expect(1).to.eq(-1);
        })

        it('return 1', () => {
            expect(1).to.eq(1);
        })
    });
});