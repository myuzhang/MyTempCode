const gmailService = require("./gamilService");
const securityManager = require("./securityManager");

exports.handler = async (event, context, callback) => {
  let data = JSON.parse(event.body);
  let email = await queryEmail(data);

  callback(null, {
    statusCode: email.status,
    headers: {
      "Content-Type": "application/xml"
    },
    body: email.content
  });
};

async function queryEmail(data)
{ 
  try {
    let searchQuery = data.queryString;
    let { timeoutSecond = 30 } = data.timeout || {};
    let gmail = gmailService(securityManager.getGamilToken(), securityManager.getGmailCredentials());
    let decodedMessage = await gmail.getEmailBody({query: searchQuery, retries: timeoutSecond});    
    return {status: 200, content: decodedMessage};
  } catch (err) {
    return {status: 500, content: err};
  }
}