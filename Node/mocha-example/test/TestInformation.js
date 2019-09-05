class TestInfo {
    constructor() {
      this.log = [];
    }
  
    set result(result) {
      this.log.push(result);
    }
  
    get results() {
      return this.log;
    }
}

module.exports = TestInfo;