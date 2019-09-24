const index = require("./index");
const gamilService = require("./gamilService");
const security = require("./securityManager");

let event = {
  body: `{"url": "https://boi-app.test.ignitionadvice.com"}`
};

describe("verify email service", () => {
  test('can get gmail', async () => {
    let searchQuery = `subject:you like`;
    let gmail = gamilService(security.getGamilToken(), security.getGmailCredentials());
    let decodedMessage = await gmail.getEmailBody({query: searchQuery, retries: 10});
    expect(decodedMessage).toEqual(expect.stringContaining("Your stuff"));    
  });
});