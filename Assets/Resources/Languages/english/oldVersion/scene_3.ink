=== scene_3_intro ===
~news_count++

Here's article number 3. 

+ [Read the article] -> s3_article
+ [Skip the article] -> s3_choice

== s3_article ==
You read the article. 
    ~news_read++
    -> s3_choice


== s3_choice == 
What do you want to do? 

+ [Send it to Group 1] You send it to group 1.
    ~group_1++
    -> s3_results
+ [Send it to Group 2] You send it to group 2.
    ~group_2++ 
    -> s3_results
+ [Send it to Group 3] You send it to group 3.
    ~group_3++
    -> s3_results
+ [Don't send it to anyone]
    -> s3_results


== s3_results == 

This is what will result from your choice. 
Number of news pieces received: {news_count}
Number of news pieces read: {news_read}

Group 1: {group_1}
Group 2: {group_2}
Group 3: {group_3}

-> scene_4_intro
