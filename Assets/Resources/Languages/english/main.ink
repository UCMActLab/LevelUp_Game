// LEVEL UP PROJECT
// PROTOTYPE TEMPLATE

// This file is a sample of how the game for the Level Up Project would flow. 

WELCOME TO OUR GAME
-> tutorial_check


// I use this file to set up all the variables and functions to keep my code neat


// These are variables that will be set based on fields from the database. These need to be updated through Unity.  

VAR checked = false   // this variable tells us whether the piece of news has been fact-checked or not. 
TODO THIS VARIABLE IS THE SAME AS THE FIELD topic IN THE DATABASE, NEED TO MAKE THEM WORK TOGETHER
VAR theme = () // this variable is another value in the list of Theme (with capital T) in database.ink 
/* Themes will be one of the following tags, and it'll be a string. The strings have to be the specific ones below. 
We may want to change the tags to make them easier to type and avoid typos. 
Tags are:
- news
- science
- conspiracy 
- computers
- scam 
*/

VAR comes_from = () // this variable keeps track of what the source of each variable is. I'm keeping track of it because I may want to show different texts depending on what the source is. 

// Variables to keep track of number of news checked

VAR this_news_read = false // this will check whether the player has read a specific piece of news)
VAR news_count = 0 // this is the number of news pieces the player has received
VAR news_read = 0 // this is the number of news pieces the player has actually stopped and read.
VAR news_fake = 0 // this is the number of fake news that have been forwarded
VAR news_checked = 0 // this is the number of fact checked news that have been forwarded
VAR article_sent = false // this keeps track of whether an article has been sent already or not. It's a patch to be able to change the option 


// Variables to help keep track of how much different groups like what we send them or say to them.

VAR group_1 = 0 // this is the variable to track the number of news the first group of people agreed with (independently of whether they are fake or not)
VAR group_2 = 0 // this is the variable to keep track of the number of news the second group agreed with
VAR group_3 = 0 // this is the variable to keep track of the number of news the third group agreed with


// Variables according to topic

INCLUDE database.ink
INCLUDE Introduction.ink
INCLUDE scene_1.ink
INCLUDE scene_2.ink
INCLUDE interlude_1.ink
INCLUDE scene_3.ink
INCLUDE scene_4.ink
INCLUDE interlude_2.ink
INCLUDE scene_5.ink




// HERE I DEFINE MY FUNCTIONS SO WE HAVE A WAY TO CHECK EACH NEWS PIECE AND WE DON'T HAVE TO REPEAT THIS EVERY SINGLE TIME

    

=== function group_1_opinion (topic, veracity)
    { 
    - topic == news && veracity == false: 
        ~ group_1++
    - topic == news && veracity == true: 
        ~group_1--
    - topic == science && veracity == false: 
        ~group_1++
    - topic == science && veracity == true: 
        ~group_1--
    - topic == conspiracy && veracity == false: 
        ~group_1++
    - topic  == conspiracy && veracity == true: 
        ~group_1--
    - topic == computers && veracity == false: 
        ~group_1++
    - topic == computers && veracity == true: 
        ~group_1--
    - topic == scam && veracity == false: 
        ~group_1++
    - topic == scam && veracity == true: 
        ~group_1--
        }


=== function group_2_opinion (topic, veracity)
    {  
    - topic == news && veracity == false: 
        ~group_2--
    - topic == news && veracity == true: 
        ~group_2++
    - topic == science && veracity == false: 
        ~group_2--
    - topic == science && veracity == true: 
        ~group_2++
    - topic == conspiracy && veracity == false: 
        ~group_2--
    - topic  == conspiracy && veracity == true: 
        ~group_2--
    - topic == computers && veracity == false: 
        ~group_2--
    - topic == computers && veracity == true: 
        ~group_2++
    - topic == scam && veracity == false: 
        ~group_2--
    - topic == scam && veracity == true: 
        ~group_2--
        }

=== function group_3_opinion (topic, veracity)
    {
    - topic == news && veracity == false: 
        ~group_3++
    - topic == news && veracity == true: 
        ~group_3--
    - topic == science && veracity == false: 
        ~group_3++
    - topic == science && veracity == true: 
        ~group_3++
    - topic == conspiracy && veracity == false: 
        ~group_3++ 
        TODO this value above should really increase or decrease at random, but I'll figure out the formula later
    - topic  == conspiracy && veracity == true: 
        ~group_3--
        TODO this should really increase or decrease at random, but I'll figure out the formula later
    - topic == computers && veracity == false: 
        ~group_3--
    - topic == computers && veracity == true: 
        ~group_3--
    - topic == scam && veracity == false: 
        ~group_3++
    - topic == scam && veracity == true: 
        ~group_3++
        }

    
TODO: Each group is going to react to trends differently. The functions below are basic examples



TODO The variable names don't work right now. I'm including the list directly, but it's a bit of a mess. Will try to fix later. 
//The variables below selects the name of a person in a specific group at random. This will be used to generate dynamic responses to the articles. 

VAR group1_names = ""
VAR group2_names = ""
VAR group3_names = ""
~ group1_names = "{~Mariah|Mark|Manny}"
~ group2_names = "{~Fred (son)|Alfie (grandson)|Felicia (granddaughter)}"
~ group3_names = "{~Paula|Omar|Emma}"

// This is a function to tell players off when they don't read the news.
 === function scold1()
{ 
    - checked == false && this_news_read == false: 
    : Did you read this before sending it?
    - checked == true && this_news_read == false:
    : Do you really believe this?
    - else:
    : That's interesting. 
    }
    
    
// This is a function to tell players off when they send more fake news than read them

=== function scold2()
{ 
    - news_read < news_fake:
    You're not really reading the news you're sending, are you. 
    - else:
    Okay, thanks for sharing. 
    }



    
