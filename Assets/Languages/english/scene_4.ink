=== scene_4_intro ===
~news_count++

Here's article number 4. 

+ [Read the article] -> s4_article
+ [Skip the article] -> s4_choice

== s4_article ==
You read the article. 
    ~news_read++
    -> s4_choice


== s4_choice == 
What do you want to do? 

+ [Send it to Group 1] You send it to group 1.
    ~group_1++
    -> s4_results
+ [Send it to Group 2] You send it to group 2.
    ~group_2++ 
    -> s4_results
+ [Send it to Group 3] You send it to group 3.
    ~group_3++
    -> s4_results
+ [Don't send it to anyone]
    -> s4_results


== s4_results == 

This is what will result from your choice. 
Number of news pieces received: {news_count}
Number of news pieces read: {news_read}

Group 1: {group_1}
Group 2: {group_2}
Group 3: {group_3}

-> scene_5_intro
