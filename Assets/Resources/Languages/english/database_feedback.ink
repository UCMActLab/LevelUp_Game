// Below is a set of functions and the database that will generate feedback to the player after they send a piece of news. Each group will have positive or negative reactions to the news.

/* The functions are below 
{feedback_group1_positive()}
{feedback_group1_negative()}
{feedback_group2_positive()}
{feedback_group2_negative()}
{feedback_group3_positive()}
{feedback_group3_negative()}
*/

// the lists below are the names of the characters divided by group

LIST group1_members = Mariah, Mark, Manny
LIST group2_members = Fred, Alfie, Felicia
LIST group3_members = Paula, Omar, Emma

=== function feedback_group1_positive()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g1_pos)) //this selects one of the corresponding feedback lines at random. 
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members)) // this selects one of the names of the people that belong to the corresponding group at random. 
{group1_speaker}: {feedback_output_group1_positive (print_feedback)}


LIST fb_lines_g1_pos = fb1_g1_pos, fb2_g1_pos, fb3_g1_pos, fb4_g1_pos, fb5_g1_pos, fb6_g1_pos, fb7_g1_pos, fb8_g1_pos, fb9_g1_pos, fb10_g1_pos //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group1_positive (feedback_lines_g1) // this is the feedback database that has the positive responses for group 1
{feedback_lines_g1:
    - fb1_g1_pos: Oh, I really want to see more of this. 
    - fb2_g1_pos: I've seen this before, but thank you for sharing with everyone.  
    - fb3_g1_pos: Happy to see there's people on the same page. 
    - fb4_g1_pos: I'm glad to see that people can learn the truth now. 
    - fb5_g1_pos: This provides confirmation to what I thought already. 
    - fb6_g1_pos: Thank you for sending this, keep them coming. 
    - fb7_g1_pos: I did not know this, but I'm pleased to know now. 
    - fb8_g1_pos: Thanks for sharing, this is good to know.
    - fb9_g1_pos: Thanks for keeping us informed, you're a good friend.  
    - fb10_g1_pos: It's good to see articles that share my perspective. 
    - else: [Cannot find positive {feedback_lines_g1} for group 1]

    }

=== function feedback_group1_negative()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g1_neg))
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
{group1_speaker}: {feedback_output_group1_negative (print_feedback)}


LIST fb_lines_g1_neg = fb1_g1_neg, fb2_g1_neg, fb3_g1_neg, fb4_g1_neg, fb5_g1_neg, fb6_g1_neg, fb7_g1_neg, fb8_g1_neg, fb9_g1_neg, fb10_g1_neg //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group1_negative (feedback_lines_g1) // this is the feedback database that has the negative responses for group 1
{feedback_lines_g1:
    - fb1_g1_neg: I do not believe this. 
    - fb2_g1_neg: Please do not send this garbage. 
    - fb3_g1_neg: This is a lie. Please do not send this to me. 
    - fb4_g1_neg: It's so upsetting. I don't think it's true. 
    - fb5_g1_neg: I do not want to read news like these. 
    - fb6_g1_neg: It has to be wrong. That's not how I see it. 
    - fb7_g1_neg: You should not distribute this, we don't like it. 
    - fb8_g1_neg: I don't agree with this. This is wrong. 
    - fb9_g1_neg: No, no, no. 
    - fb10_g1_neg:I don't like reading these things. 
    - else: [Cannot find positive {feedback_lines_g1} for group 1]

    }

=== function feedback_group2_positive()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g2_pos))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
{group2_speaker}: {feedback_output_group2_positive(print_feedback)}


LIST fb_lines_g2_pos = fb1_g2_pos, fb2_g2_pos, fb3_g2_pos, fb4_g2_pos, fb5_g2_pos, fb6_g2_pos, fb7_g2_pos, fb8_g2_pos, fb9_g2_pos, fb10_g2_pos //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group2_positive (feedback_lines_g2) // this is the feedback database that has the positive responses for group 2
{feedback_lines_g2:
    - fb1_g2_pos: Yes, thank you.
    - fb2_g2_pos: Good to see you're keeping up with the news. 
    - fb3_g2_pos: I've seen it before, but thanks for sharing with all of us. 
    - fb4_g2_pos: Yes, I've seen a similar article, thanks for sharing. 
    - fb5_g2_pos: My friends told me about this, this article confirms it. 
    - fb6_g2_pos: Thanks for sharing. I'm glad to see youn engaged with the news. 
    - fb7_g2_pos: This is excellent, thank you. 
    - fb8_g2_pos: Thank you for keeping us informed. 
    - fb9_g2_pos: I had read this already, but it's a good reminder. 
    - fb10_g2_pos:Thank you for making us aware. 
    - else: [Cannot find positive {feedback_lines_g2} for group 1]

    }
    
    
=== function feedback_group2_negative()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g2_neg))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
{group2_speaker}: {feedback_output_group2_negative (print_feedback)}


LIST fb_lines_g2_neg = fb1_g2_neg, fb2_g2_neg, fb3_g2_neg, fb4_g2_neg, fb5_g2_neg, fb6_g2_neg, fb7_g2_neg, fb8_g2_neg, fb9_g2_neg, fb10_g2_neg //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group2_negative (feedback_lines_g2) // this is the feedback database that has the negative responses for group 2
{feedback_lines_g2:
    - fb1_g2_neg: Please don't send these things. 
    - fb2_g2_neg: I don't think this is true. 
    - fb3_g2_neg: People keep sending me this article and it's wrong. 
    - fb4_g2_neg: This is so biased, I don't want to read it. 
    - fb5_g2_neg: What a misleading article!
    - fb6_g2_neg: This is nonsense, you can just tell by the headline. 
    - fb7_g2_neg: Is this what you're reading? Please be careful. 
    - fb8_g2_neg: This has been disproven already, don't spread it. 
    - fb9_g2_neg: The article is so manipulated! 
    - fb10_g2_neg: People invent terrible things to spread out. 
    - else: [Cannot find positive {feedback_lines_g2} for group 2]

    }
    
=== function feedback_group3_positive()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g3_pos))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))
{group3_speaker}: {feedback_output_group3_positive(print_feedback)}


LIST fb_lines_g3_pos = fb1_g3_pos, fb2_g3_pos, fb3_g3_pos, fb4_g3_pos, fb5_g3_pos, fb6_g3_pos, fb7_g3_pos, fb8_g3_pos, fb9_g3_pos, fb10_g3_pos //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group3_positive (feedback_lines_g3) // this is the feedback database that has the positive responses for group 3
{feedback_lines_g3:
    - fb1_g3_pos: Nice! Thanks. 
    - fb2_g3_pos: How interesting! Thanks for sharing. 
    - fb3_g3_pos: I did not know this.
    - fb4_g3_pos: Saving this for later, thank you. 
    - fb5_g3_pos: I wanted to know more about this, yes. Thanks.
    - fb6_g3_pos: I've been hearing about this, yes. 
    - fb7_g3_pos: Thank you for sending this. 
    - fb8_g3_pos: Ah, good. Thanks. 
    - fb9_g3_pos: This is a good find, thank you. 
    - fb10_g3_pos:This is food for thought, I appreciate it. 
    - else: [Cannot find positive {feedback_lines_g3} for group 1]

    }

=== function feedback_group3_negative()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g3_neg))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))
{group3_speaker}: {feedback_output_group3_negative (print_feedback)}


LIST fb_lines_g3_neg = fb1_g3_neg, fb2_g3_neg, fb3_g3_neg, fb4_g3_neg, fb5_g3_neg, fb6_g3_neg, fb7_g3_neg, fb8_g3_neg, fb9_g3_neg, fb10_g3_neg //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group3_negative (feedback_lines_g3) // this is the feedback database that has the negative responses for group 1
{feedback_lines_g3:
    - fb1_g3_neg: These news make me uncomfortable. 
    - fb2_g3_neg: I don't like this article. 
    - fb3_g3_neg: Why did you send this?
    - fb4_g3_neg: Don't know what to believe anymore. 
    - fb5_g3_neg: What a world we live in. 
    - fb6_g3_neg: Very concerning indeed. 
    - fb7_g3_neg: What a scary world we live in!
    - fb8_g3_neg: I feel very confused right now. 
    - fb9_g3_neg: I don't find this very convincing. 
    - fb10_g3_neg:Doesn't seem to add up. 
    - else: [Cannot find positive {feedback_lines_g3} for group 2]

    }

   
    