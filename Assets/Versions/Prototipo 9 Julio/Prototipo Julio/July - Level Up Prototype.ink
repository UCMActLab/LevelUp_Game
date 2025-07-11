// LEVEL UP PROJECT
// PROTOTYPE TEMPLATE

// This file is a sample of how the game for the Level Up Project would flow. 

WELCOME TO TO OUR GAME
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

//The variables below keep track of how many articles the player has sent to them. 
VAR article_forwarded_group1 = 0
VAR article_forwarded_group2 = 0
VAR article_forwarded_group3 = 0



// Variables to help keep track of how much different groups like what we send them or say to them.

VAR group_1 = 0 // this is the variable to track the number of news the first group of people agreed with (independently of whether they are fake or not)
VAR group_2 = 0 // this is the variable to keep track of the number of news the second group agreed with
VAR group_3 = 0 // this is the variable to keep track of the number of news the third group agreed with


// Variables according to topic

INCLUDE database_hardcoded.ink
INCLUDE database_dynamic.ink
INCLUDE database_feedback.ink
INCLUDE Introduction.ink
INCLUDE scene_1.ink
INCLUDE scene_2.ink
INCLUDE interlude_1.ink
INCLUDE scene_3.ink
INCLUDE scene_4.ink
INCLUDE interlude_2.ink
INCLUDE scene_5.ink
INCLUDE scene_0.ink
INCLUDE ending.ink
INCLUDE scene_1b.ink
INCLUDE scene_3b.ink





// The lists and variables below all set up the database strucutres for the article databases. 

LIST Data = headline, sources, body, Theme, verified // these are the fields of each database item

LIST source = blog, newspaper, social // these are the list of labels to mark the source.

LIST themes = news, science, conspiracy, computers, scam // these are the labels to mark the topics that news can be about

~ temp print_article = LIST_RANDOM(LIST_ALL(articles))

// These are the functions that set up and contain the data

=== function data (what, headline_data, source_data, body_data, theme_data, verified_data) // this function sets up the database entry fields
{ what:
    - headline: ~ return headline_data
    - sources: ~ return source_data
    - body: ~ return body_data
    - Theme: ~ return theme_data
    - verified: ~ return verified_data
}

// The function below is to print out the source of the article, to facilitate both the labels in the database and localization. 

=== function display_source()
{ 
    - comes_from == blog:
        ~ return ("This is a blog post.")
    - comes_from == newspaper:
        ~ return ("This is a newspaper article.")
  - else:
        ~ return ("This is a social media post.")
}


// HERE I DEFINE MY FUNCTIONS SO WE HAVE A WAY TO CHECK EACH NEWS PIECE AND WE DON'T HAVE TO REPEAT THIS EVERY SINGLE TIME

    

=== function group_1_opinion (topic, veracity)
    { 
    - topic == news && veracity == false: 
        ~ group_1++
        ~ feedback_group1_positive()

    - topic == news && veracity == true: 
        ~group_1--
        ~feedback_group1_negative()
    - topic == science && veracity == false: 
        ~group_1++
        ~feedback_group1_positive()
    - topic == science && veracity == true: 
        ~group_1--
        ~feedback_group1_negative()
    - topic == conspiracy && veracity == false: 
        ~group_1++
        ~feedback_group1_positive()
    - topic  == conspiracy && veracity == true: 
        ~group_1--
        ~feedback_group1_negative()
    - topic == computers && veracity == false: 
        ~group_1++
        ~feedback_group1_positive()
    - topic == computers && veracity == true: 
        ~group_1--
        ~feedback_group1_negative()
    - topic == scam && veracity == false: 
        ~group_1++
        ~feedback_group1_positive()
    - topic == scam && veracity == true: 
        ~group_1--
        ~feedback_group1_negative()

        }


=== function group_2_opinion (topic, veracity)
    {  
    - topic == news && veracity == false: 
        ~group_2--
        ~feedback_group2_negative()
    - topic == news && veracity == true: 
        ~group_2++
        ~feedback_group2_positive()
    - topic == science && veracity == false: 
        ~group_2--
        ~feedback_group2_negative()
    - topic == science && veracity == true: 
        ~group_2++
        ~feedback_group2_positive()
    - topic == conspiracy && veracity == false: 
        ~group_2--
        ~feedback_group2_negative()
    - topic  == conspiracy && veracity == true: 
        ~group_2--
        ~feedback_group2_negative()
    - topic == computers && veracity == false: 
        ~group_2--
        ~feedback_group2_negative()
    - topic == computers && veracity == true: 
        ~group_2++
        ~feedback_group2_positive()
    - topic == scam && veracity == false: 
        ~group_2--
        ~feedback_group2_negative()
    - topic == scam && veracity == true: 
        ~group_2--
        ~feedback_group2_negative()
        }

=== function group_3_opinion (topic, veracity)
    {
    - topic == news && veracity == false: 
        ~group_3++
        ~feedback_group3_positive()
    - topic == news && veracity == true: 
        ~group_3--
        ~feedback_group3_negative()
    - topic == science && veracity == false: 
        ~group_3++
        ~feedback_group3_positive()
    - topic == science && veracity == true: 
        ~group_3++
        ~feedback_group3_positive()
    - topic == conspiracy && veracity == false: 
        ~group_3++ 
        ~feedback_group3_positive()
        TODO this value above should really increase or decrease at random, but I'll figure out the formula later
    - topic  == conspiracy && veracity == true: 
        ~group_3--
        ~feedback_group3_negative()
        TODO this should really increase or decrease at random, but I'll figure out the formula later
    - topic == computers && veracity == false: 
        ~group_3--
        ~feedback_group3_negative()
    - topic == computers && veracity == true: 
        ~group_3--
        ~feedback_group3_negative()
    - topic == scam && veracity == false: 
        ~group_3++
        ~feedback_group3_positive()
    - topic == scam && veracity == true: 
        ~group_3++
        ~feedback_group3_positive()
        }
    
TODO: Each group is going to react to trends differently - there needs to be a separate function per group, similar to the ones that give feedback.  The functions below are basic examples


// The functions below are for the family to tell off the player. 

 === function scold1_group2()
 ~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
   {
    - checked == false && this_news_read == false: 
    {group2_speaker}: Did you read this before sending it?
    - checked == true && this_news_read == false:
    {group2_speaker}:: Do you really believe this?
    - else:
    {group2_speaker}:: That's interesting. 
    }

    
    
// This is a function to tell players off when they send more fake news than read them

=== function scold2_group2()
 ~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
{ 
    - news_read < news_fake:
    {group2_speaker}: You're not really reading the news you're sending, are you. 
    - else:
    {group2_speaker}: Okay, thanks for sharing. 
    }






    
