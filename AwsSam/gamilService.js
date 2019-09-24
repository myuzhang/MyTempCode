const { google } = require("googleapis");

module.exports = function(token, credentials) {

  function getAuth()
  {
    const { client_secret, client_id, redirect_uris } = credentials.installed || {};
    const oAuth2Client = new google.auth.OAuth2(client_id, client_secret, redirect_uris[0]);
    oAuth2Client.setCredentials(token);
    return oAuth2Client;
  }

  async function getEmailBody(spec)
  {
    let auth = getAuth();
    let messageDate = await getMessageData(auth, spec);
    let gmailContent = await getMessageDataById(auth, messageDate);
    let message = gmailContent.payload.body.data;
    let decodedMessage = new Buffer.from(message, "base64").toString();
    return decodedMessage;
  }

  /**
 * Get the list of emails from your Gmail account (object contains ID)
 * @param {google.auth.OAuth2} auth An authorized OAuth2 client.
 * @return {auth: authorised client, response: response of gmail.users.messages.list}
 */
  function getMessageData(auth, spec, incrementer = 0) {
    return new Promise((resolve, reject) => {
      const gmail = google.gmail({ version: "v1", auth: auth });
      // Retrieve mail list by query
      gmail.users.messages.list(
        {
          auth: auth,
          userId: "me",
          labelIds: ["INBOX"],
          q: spec.query
        },
        function(err, response) {
          if (err) {
            return reject(err);
          } else {
            if (response && response.data && response.data.resultSizeEstimate) {
              resolve(response.data);
            } else if (incrementer < spec.retries) {
              setTimeout(() => {
                return getMessageData(auth, spec, incrementer + 1).then(resolve, reject);
              }, 1000);
            } else {
              reject(`Retrieving Gmail failed after ${spec.retries} retries.`);
            }
          }
        }
      );
    });
  }

  /**
   * Get the recent email from your Gmail account
   * @param {google.auth.OAuth2} auth An authorized OAuth2 client.
   * @return {auth: authorised client, response: response of gmail.users.messages.list}
   */
  function getMessageDataById(auth, data) {
    return new Promise((resolve, reject) => {
      const gmail = google.gmail({ version: "v1", auth });
      gmail.users.messages.get(
        {
          auth: auth,
          userId: "me",
          id: data.messages[0].id
        },
        function(err, response) {
          if (err) {
            reject("Error: " + err);
          } else {
            resolve(response.data);
          }
        }
      );
    });
  }

  return Object.freeze({getEmailBody});
};