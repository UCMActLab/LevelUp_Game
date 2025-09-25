=== scene_1_intro ===
TODO: DO WE WANT TO INCLUDE WHO IS SENDING THE ARTICLE

~ news_count++
~ article_sent = false

TODO The line is the line that selects the article from the database. This should hook into the external database. 

~ temp print_article = HC_art1 // this is the line where I'm selecting a specific article from the hardcoded database
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
~ comes_from = article_data_HC (print_article, sources)

ARTICLE RECEIVED
Article headline : {article_data_HC (print_article, headline)}
TODO: These values need to be taken from the database - this will be tags for each piece of news. The database also needs at least a headline and a text body. This code will be the same in each scene. 

+ [Read the article] -> s1_article
+ [Skip the article] -> s1_choice

=== s1_article ===
    ~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC(print_article, body)} -> s1_choice

== s1_choice == 
TODO:  Still need to fix the last option so the game moves to the next knot when all options have been exhausted. I fixed the last option, so it checks whether the article has been sent already or not and displays the right text, but this may have issues when translating the game to other languages -- it's not good practice for localization. 

* [Send it your friends.] 
    ~ article_forwarded_group1++
    ~ article_sent = true
   -> s1_g1

* [Send it to your family.] 
    ~ article_forwarded_group2++
    ~ article_sent = true

    -> s1_g2
    
* [Send it to your neighbours.] 
    ~ article_forwarded_group3++
    ~ article_sent = true

    -> s1_g3
* [Don't send it to anyone{article_sent: else}.] -> s1_results
    
== s1_g1 ==
Sent to Friends Group.
~ group_1_opinion(theme, checked)
Mariah: I had seen this already, John just sent it to us. It's outrageous that they want to take away our hard-earned pensions.
    -> s1_choice
    
== s1_g2 ==
Sent to Family Group.
    ~ group_2_opinion(theme, checked)
Fred: You don't believe this, do you? This is is a scam, please do not fall for it. They just want your data. The government is not offering any financial support. 
{scold1_group2()}
     -> s1_choice

== s1_g3 == 
Sent to Neighbours Group.
    ~ group_3_opinion(theme, checked)
Paula: Oh, thank you! We lost all the food in the freezer at our bar, we can certainly use this.  
    -> s1_choice

== s1_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> scene_1b_intro
