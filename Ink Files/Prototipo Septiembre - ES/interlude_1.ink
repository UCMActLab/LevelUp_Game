=== checkin_1 === 
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

{article_forwarded_group1 == 0: {group1_speaker}: Hace tiempo que no te vemos por el chat, ¿sigues ahí? }
{article_forwarded_group2 == 0: {group2_speaker}: ¿Estás bien? No sabemos de ti.}
{article_forwarded_group3 == 0: {group3_speaker}: Ya no nos mandas nada, ¿todo bien?  }

Estas son las actitudes de cada grupo hacia ti. 

TODO These variables are just for the prototype. We need a function to calculate the mood trends, but we need more items in the database for that. 
TODO Rather than reporting this in text, it'd be better to show it in a counter / bar on top of the screen, similar to what Reigns does. 

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

//The text below are the consequence for sending specific news to specific teams. This is hardcoded, so it can only be done with certain pieces of news. 
{s1_g3: Paula: He tenido que ir al banco. Mandé mis datos para recibir compensación por las pérdidad del apagón y me han robado los dados. ¡No utiliceis el link, es un timo!}
{s2_g1:
Marcos: Me voy a quedar en casa. Creo que cogí el COVID cuando fui al partido el otro día.
} 

-> scene_3_intro