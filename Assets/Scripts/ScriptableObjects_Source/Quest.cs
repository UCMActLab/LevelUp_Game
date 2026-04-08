using UnityEngine;

public class Quest : ScriptableObject
{
    // TO DO
    public struct ToDo
    {
        public int falseArticlesToSkip;

        public int articlesToIdentify; // identify/share

        public int toShareWithFriends;
        public int toShareWithNeighbours;
        public int toShareWithFamily;

        public int toRead; // articles to read
    }

    public int totalArticles;

    public int GetMaxPossibleScore() { return this.toDo.articlesToIdentify + this.toDo.toShareWithNeighbours + this.toDo.toShareWithFriends + this.toDo.toShareWithFamily; }

    public void BuildQuest(int numTrueArticles, int totalArticles, int numGroups, int articlesToRead)
    {
        this.totalArticles = totalArticles;

        toDo.articlesToIdentify = numTrueArticles;
        toDo.falseArticlesToSkip = this.totalArticles - numTrueArticles;

        if (numGroups > 0)
        {
            for (int i = 0; i < toDo.articlesToIdentify; ++i)
            {
                int rand = Random.Range(0, numGroups + 1);
                if (rand == 0) toDo.toShareWithFamily++;
                if (rand == 1) toDo.toShareWithFriends++;
                if (rand == 2) toDo.toShareWithNeighbours++;
            }
        }

        toDo.toRead = articlesToRead;
    }

    public void EvaluateArticle(ArticleGameObject data)
    {
        if (data.HasSharedArticle)
        {
            // add "with wich group have we shared?"

            if(data.IsTrue)
            {
                done.identifiedArticles++;
            }
            else
            {
                done.falseArticlesShared++;
            }

            Debug.LogError("TODO: SharingPoints");
            
            //                  TODO
            // ============================================

            // sharedWithFriends
            // sharedWithNeighbours
            // sharedWithFamily

            // ============================================
        }

        if (data.HasReadArticle) done.readedArticles++;
    }

    // DONE 
    public struct Done
    {
        public int identifiedArticles; // positive
        public int falseArticlesSkipped; // positive
        public int sharedWithFriends; // positive
        public int sharedWithNeighbours; // positive
        public int sharedWithFamily; // positive
    
        public int falseArticlesShared; // negative

        public int readedArticles; // neutral -> doesn't awards points
    }

    public ToDo toDo;
    public Done done;
}
