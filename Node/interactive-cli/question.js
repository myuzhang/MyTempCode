const { askQuestions } = require("./readlines.js"););

module.exports = {
  getFromAnswers: async () => {
      let answers = await askQuestions([
        "What is the name> ",
        "What is the email> ",
        "What is the password> "
      ]);
	  
	  console.log(`Your name: {answers[0]}`);
  }
};