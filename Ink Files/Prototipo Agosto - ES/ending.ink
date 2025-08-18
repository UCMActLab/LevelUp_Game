Here the game tells the player how they're doing. 

TODO These variables are just for the prototype. We need a function to calculate the mood trends, but we need more items in the database for that. 

== game_ending ==
// Final reaction of group 1
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

TODO IF THE PLAYER HAS NOT FORWARDED ANY NEWS, THE FAMILY SHOULD BE CHECKING ON THEM - AND OTHER GROUPS SHOULD NOTE THAT THE PLAYER HAS BEEN VERY QUIET.


{article_forwarded_group1 == 0: {group1_speaker}: You're a bit quiet.}
{article_forwarded_group2 == 0: {group2_speaker}: Are you okay?}
{article_forwarded_group3 == 0: {group3_speaker}: Haven't heard from you in a bit. }




{   
    - group_1 <= -2:
    {group1_speaker}: Creo que no ves las cosas como son. No me fío de ti.  
    - group_1 >= 2:
    {group1_speaker}: No hagas caso de lo que te digan otros. El mundo está lleno de mentirosos. 
  - else:
    {group1_speaker}: Espero que estéis todos bien.  
}

// Final reaction of group 2
{   
    - group_2 <= -2:
    {group2_speaker}: Nos tienes preocupados. Nos has mandado artículos que son bulos y no queremos que te engañen.
    - group_2 >= 2:
    {group2_speaker}: Nos hace ilusión que nos mandes artículos útiles. Parece que estás al día y tienes cuidado con las fuentes.
  - else:
    {group2_speaker}: ¿Cuándo te vamos a ver?
}

// Final reaction of group 3

{   
    - group_3 <= -2:
    {group3_speaker}: Ten cuidado con lo que compartes. Hay cosas que no parecen de confianza. 
    - group_3 > 2:
    {group3_speaker}: Sigue mandándonos cosas, tus artículos nos han sido muy útiles. 
  - else:
    {group3_speaker}: ¡Qué bueno hace hoy! Deberíamos guardar los teléfonos y dar un paseo. 
    }

El grupo de amigos <>{
    - group_1 >= 1:
     está a gusto contigo.
    - group_1 == 0:
     son indiferentes a lo que digas.  
    - else:
    recelan de lo que les mandas.  
}
Tu familia <>{
    - group_2 >= 1:
     está tranquila por ti.
    - group_2 == 0:
     ni se preocupan ni se despreocupan por ti.
    - else:
     está preocupada por ti. 
}

Tus vecinos <>{
    - group_3 >= 1:
     están contentos de tenerte en el grupo. 
    - group_3 == 0:
     no tiene mucha opinión de tu participación en el grupo. 
    - else:
     están un poco incómodos contigo.  
}


Has llegado al final de la historia. 
GRACIAS POR JUGAR -> END
