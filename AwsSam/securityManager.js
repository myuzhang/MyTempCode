module.exports = {
  getGmailCredentials: () => {
    let credentialsString = process.env.gmailCredentials;
    if (!credentialsString) {
      throw new Error("Please set gmail credentials in env variable as gmailCredentials");
    }
    return JSON.parse(credentialsString);
  },
  getGamilToken: () => {
    let tokenString = process.env.gmailToken;
    if (!tokenString) {
      throw new Error("Please set gmail token in env variable as gmailToken");
    }
    return JSON.parse(tokenString);
  }
};