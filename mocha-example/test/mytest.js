const assert = require('chai').assert;
const expect = require('chai').expect;
const should = require('chai').should;

describe('array', () => {

    before(() => {
        console.log('1-block1 before1');
      })
    
    before(() => {
    console.log('1-block1 before2');
    })
      
    after(() => {
    console.log('1-block1 after');
    })

    beforeEach(() => {
        console.log('1-block1 before each');
    });

    describe('indexof()', () => {

        before(() => {
            console.log('1-block2 before');
          })
    
        beforeEach(() => {
            console.log('1-block2 before each');
        });

        it('should return -1', () => {
            expect(1).to.eq(1);
        })

        it('should return 1', () => {
            expect(1).to.eq(1);
        })
    });
});