import json
import logging
import configparser
import requests
import datetime
import os
from slackclient import SlackClient

logger = logging.getLogger()
logger.setLevel(logging.INFO)

class Card:
    def __init__(self, cardId, title, assignee, columnName):
        self.cardId = cardId
        self.title = title
        self.assignee = assignee
        self.columnName = columnName

class BlockedCard(Card):
    def __init__(self, cardId, title, assignee, columnName, blockedTime):
        Card.__init__(self, cardId, title, assignee, columnName)
        self.blockedTime = blockedTime

class WipCard(Card):
    def __init__(self, cardId, title, assignee, columnName, deadline, leadTimeInDays):
        Card.__init__(self, cardId, title, assignee, columnName)
        self.deadline = deadline
        self.leadTimeInDays = leadTimeInDays

def getSettings(section, key):
    """ Retrieve settings from Settings.ini """
    config = configparser.ConfigParser()
    config.read("Settings.ini")
    return config[section][key]

def getAssigneeOrAlternative(task):
    dev = "none"
    qa = "none"
    vm = "none" 
    if (task["assignee"].lower() == "none" or task["assignee"].lower() == "external"):
        for field in task["customfields"]:
            if (field["name"] == "Delivery Leader - Dev"):
                if (field["value"].lower() != "none"):
                    dev = field["value"]
            if (field["name"] == "Delivery Leader - QA"):
                if (field["value"].lower() != "none"):
                    qa = field["value"]
            if (field["name"] == "Value Master"):
                if (field["value"].lower() != "none"):
                    vm = field["value"]

    if dev != "none":
        return dev
    elif qa != "none":
        return qa
    elif vm != "none":
        return vm
    else:
        return task["assignee"]

def getSlackUser(kanbanizeName):
    """ Map user name from kanbanize to Slack """
    try:
        return getSettings('Kanbanize2Slack4Username', kanbanizeName)
    except:
        return kanbanizeName

def truncate(originalString, tail = '...', expectedLength = 100):
    """ Truncate the string if the length is more than expectedLength and add ... to the end of string """
    tailLength = len(tail)
    if (len(originalString) <= expectedLength - tailLength):
        return originalString
    else:
        return originalString[:expectedLength - tailLength] + tail

def GetDiffWorkDay(diffDay):
    """ Get the number of days according to diffDay which is work day excluding weekend """
    i = 1
    weekend = 0

    if datetime.datetime.today().weekday() == 5: # if today is Saturday, skip Sunday
        weekend += 1

    while i <= diffDay:
        nextDay = datetime.datetime.today() + datetime.timedelta(i)
        if nextDay.weekday() == 5: weekend += 2 # if next day is Saturday, skip Saterday and Sunday
        i += 1

    return diffDay + weekend

def main(event, context):
    """ Retrieve cards which are blocked and wip and post them into slack channel """
    noUpdateDays = os.getenv('noUpdateDays')
    if noUpdateDays:
        diffDay = int(noUpdateDays)
    else:
        diffDay = int(getSettings('User', 'noUpdateDays'))

    noUpdateWeekDays = diffDay
    diffDay = GetDiffWorkDay(diffDay)

    slackChannel = os.getenv('slackChannel')
    if not slackChannel:
        slackChannel = getSettings('User', 'slackChannel')

    url = getSettings('Kanbanize', 'url')
    data = {'boardid':getSettings('Kanbanize', 'boardid')}
    headers = {
        'Accept': 'application/json',
        'apikey': getSettings('Kanbanize', 'apikey'),
        'ContentType': 'application/json'
    }
    response = requests.post(url, data=json.dumps(data), headers=headers)
    jAllTasks = json.loads(response.content)

    now = datetime.datetime.now()

    wipCardList = []
    blockedCardList = []

    for task in jAllTasks:
        if (task["columnname"].lower() == "qa in progress" or task["columnname"].lower() == "dev in progress" or task["columnname"].lower() == "pr in progress" or task["columnname"].lower() == "qa ready") and \
        "JDI" not in task["lanename"]:

            user = getAssigneeOrAlternative(task)
            assignee = getSlackUser(user)

            wip = WipCard(task["taskid"], task["title"], user, task["columnname"], task["deadline"], task["leadtime"])
            wipCardList.append(wip)

            updateDate = datetime.datetime.strptime(task["updatedat"], '%Y-%m-%d %H:%M:%S') + datetime.timedelta(days=diffDay)
            if now > updateDate:
                blocked = BlockedCard('*' + task["taskid"] + '*', '_' + task["title"] + '_', ':old_key: <@' + assignee + '>', '[_' + task["columnname"] + '_]', ':clock3: Since ' + task["updatedat"])
                blockedCardList.append(blocked)

    wipCardListLength = len(wipCardList)
    blockedCardListLength = len(blockedCardList)

    if wipCardListLength > 0:
        wipCardList = sorted(wipCardList, key = lambda x: x.leadTimeInDays, reverse = True)
    if blockedCardListLength > 0:
        blockedCardList = sorted(blockedCardList, key = lambda x: x.assignee)

    message = []
    message.append(f"====*Here are {blockedCardListLength} cards not being updated for more than {noUpdateWeekDays} days:*====")
    if blockedCardListLength == 0:
        message.append("None :tada:")

    for card in blockedCardList:
        cardMessage = card.cardId + ' ' + card.title + ' ' + card.assignee + ' ' + card.columnName + ' ' + card.blockedTime
        message.append(cardMessage)

    message.append(" ")

    showWip = getSettings('FeatureToggle', 'showWip')
    if showWip == True:
        message.append(f"====*Here are {wipCardListLength} cards info which are WIP:*====")
        if wipCardListLength == 0:
            message.append("None :tada:")

        message.append("`| Card | Title                                                                                               | Assignee        | Column Name      | Deadline  | Leadtime (days) |`")
        for card in wipCardList:
            if card.deadline is None:
                card.deadline = ' '

            title = truncate(card.title, expectedLength=100)
            assignee = truncate(card.assignee, expectedLength=16)
            cardMessage = '`| ' + card.cardId + ' '*(5-len(card.cardId)) + \
                        '| ' + title + ' '*(100-len(title)) + \
                        '| ' + assignee + ' '*(16-len(assignee)) + \
                        '| ' + card.columnName + ' '*(17-len(card.columnName)) + \
                        '| ' + card.deadline + ' '*(10-len(card.deadline)) + \
                        '| ' + str(card.leadTimeInDays) + ' '*(15-len(str(card.leadTimeInDays))) + ' |`'
            message.append(cardMessage)

    slackMessage = '\n'.join(message)

    sc = SlackClient(getSettings('Slack', 'token'))
    sc.api_call(
    getSettings('Slack', 'method'),
    channel=slackChannel,
    text=slackMessage
    )

def lambda_handler(event, context):
    """ Lambda handler for AWS calling """
    try:
        logger.info("Retrieving Kanban card and posting to slack ...")
        main(event, context)
        logger.info("Done.")
    except Exception as e:
        logger.error(f"Run kanbanize card to slack failed ... at {str(e)}")

if __name__ == "__main__":
    lambda_handler(None, None)
