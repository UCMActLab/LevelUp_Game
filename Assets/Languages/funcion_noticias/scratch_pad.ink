INCLUDE scratch_pad2.ink

VAR group_1 = 0
VAR theme = "nothing"
VAR checked = false


Testing my variables. 

This piece of nes deals with computers and is false. 

+ [Let's see if it works.] -> temperature

== temperature ==

~ theme = "computers"
~ checked = true

~ group_1_opinion (theme, checked)

Show me {group_1}.
Show me {theme}.
Show me {checked}

            -> endithere
            

=== function group_1_opinion (topic, veracity)
    { 
    - topic == "news" && veracity == false: 
        ~group_1++
    - topic == "news" && veracity == true: 
        ~group_1--
    - topic == "science" && veracity == false: 
        ~group_1++
    - topic == "science" && veracity == true: 
        ~group_1--
    - topic == "conspiracy" && veracity == false: 
        ~group_1++
    - topic  == "conspiracy" && veracity == true: 
        ~group_1--
    - topic == "computers" && veracity == false: 
        ~group_1++
    - topic == "computers" && veracity == true: 
        ~group_1--
    - topic == "scamm" && veracity == false: 
        ~group_1++
    - topic == "scam" && veracity == true: 
        ~group_1--
        }
        
        
