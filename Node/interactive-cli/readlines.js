const readline = require("readline");

// Please refer to stackoverflow: How to take two consecutive input with the readline module of node.js?

const askQuestion = (rl, question) => {
  return new Promise(resolve => {
    rl.question(question, answer => {
      resolve(answer);
    });
  });
};

const ask = async function(questions) {
  let rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
  });

  let results = [];

  for (let i = 0; i < questions.length; i++) {
    const result = await askQuestion(rl, questions[i]);
    results.push(result);
  }

  rl.close();

  return results;
};

module.exports = {
  askQuestions: ask
};
