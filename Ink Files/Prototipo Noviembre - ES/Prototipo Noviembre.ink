// LEVEL UP PROJECT
// PROTOTYPE TEMPLATE

// This file is a sample of how the game for the Level Up Project would flow. 

BIENVENIDOS A F.A.B.U.S. #parrafo

-> Introduction

// I use this file to set up all the variables and functions to keep my code neat

// These are variables that will be set based on fields from the database. These need to be updated through Unity.  
VAR checked = false   // this variable tells us whether the piece of news has been fact-checked or not. 

TODO THE VARIABLE BELOW IS THE SAME AS THE FIELD topic IN THE DATABASE, NEED TO MAKE THEM WORK TOGETHER
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

VAR comes_from = "source" // this variable keeps track of what the source of each variable is. I'm keeping track of it because I may want to show different texts depending on what the source is. I'm declaring it as a string that is not used, so when it's updated it reads it like a string (else it gives me an error, see https://intfiction.org/t/can-not-call-use-operation-on-string-and-list/71783 )

// Variables to keep track of number of news checked are below

VAR this_news_read = false // this will check whether the player has read a specific piece of news)
VAR news_count = 0 // this is the number of news pieces the player has received
VAR news_read = 0 // this is the number of news pieces the player has actually stopped and read.
VAR news_fake = 0 // this is the number of fake news that have been forwarded
VAR news_checked = 0 // this is the number of fact checked news that have been forwarded
VAR article_sent = false // this keeps track of whether an article has been sent already or not. It's a patch to be able to change the option. it only keeps track of whether it's been sent, but not how many times. 

//The variables below keep track of how many articles the player has sent to them. 
VAR article_forwarded_group1 = 0
VAR article_forwarded_group2 = 0
VAR article_forwarded_group3 = 0

// Variables below help keep track of how much different groups like what we send them or say to them.

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
LIST source = blog, newspaper, social 
// these are the list of labels to mark the source. It is used to print the source of the piece of news, but the list itself has stopped working for VAR comes_from because it needs strings to work. That´s why this is commented out, although it worked before. (somehow the database doesn't break even if this list is not active, go figure)

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
    - comes_from == newspaper:
        ~ return ("Fuente: Periódico")
    - comes_from == social:
        ~ return ("Fuente: Redes Sociales")
    - else:
        ~ return ("Fuente: Blog")
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





    
