 private static void WrongStartingKarmaCap(ILContext il)
{
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdsfld("Expedition.ExpeditionGame","tempKarma"),
                x => x.MatchStfld("DeathPersistentSaveData", "karma")
                /*x => x.MatchLdarg(0),
                x => x.MatchLdfld("SaveState", "deathPersistentSaveData"),
                x => x.MatchLdcI4(4),
                x => x.MatchStfld("DeathPersistentSaveData", "karmaCap")*/
                ))
            {
    //Ldc.i4.number stupid
    //c.Index += 2;
    Debug.Log(il.Body);
    Debug.Log(il.Method);
    Debug.Log(il.Module);


    c.Remove();
    c.Emit(OpCodes.Ldc_I4, 8); //Karma
    Debug.Log(c);
    c.Index += 3; Debug.Log(c);
    c.Next.OpCode = OpCodes.Ldc_I4;
    c.Next.Operand = 9; //Karma Cap
    Debug.Log(c);
    Debug.Log("aaa : Karma Cap IL Success");
}
            else
{
    Debug.Log("aaa : Karma Cap IL Failed");
}
        }


private static void StartWithStomachPearlIL(ILContext il)
{
    ILCursor c = new(il);
    if (c.TryGotoNext(MoveType.Before,
    x => x.MatchLdloca(72),
    x => x.MatchLdarg(0),
    x => x.MatchCallOrCallvirt("Room", "get_abstractRoom")
    ))
    {
        Debug.Log(c);
        c.Index += 2;
        Debug.Log(c);
        //c.Emit(OpCodes.Ldarg_0);
        Debug.Log(c);
        c.EmitDelegate<Func<Room, Room>>((room) =>
        {
            //room.game.FirstRealizedPlayer.objectInStomach = new DataPearl.AbstractDataPearl(room.world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, new WorldCoordinate(room.abstractRoom.index, -1, -1, 0), room.game.GetNewID(), -1, -1, null, DataPearl.AbstractDataPearl.DataPearlType.Red_stomach);
            return room;
        });
        Debug.Log(c);
        //Debug.Log("aaa: Saint Murder Points Success"); //could be improved
    }
    else
    {
        Debug.Log("aaa:222222222222 Fail");
    }
}


/*
 * 
 *         private static void StoryGameSession_ctor(ILContext il)
c.TryGotoNext(MoveType.Before,
x => x.MatchLdfld("DeathPersistentSaveData", "karma"));

Debug.Log(c);
c.Index -= 3; Debug.Log(c);
c.RemoveRange(7); Debug.Log(c);
c.Emit(OpCodes.Ldc_I4, 9);
Debug.Log(c);

c.TryGotoNext(MoveType.Before,
x => x.MatchStfld("DeathPersistentSaveData", "karmaCap"));

Debug.Log(c);
c.Index -= 1; Debug.Log(c);
c.Next.OpCode = OpCodes.Ldc_I4;
c.Next.Operand = 9;
Debug.Log(c);
*/

//x => x.MatchLdfld("StoryGameSession", "saveState"),
//x => x.MatchLdfld("SaveState", "miscWorldSaveData"),

/*
c.Index += 1;
c.Emit(OpCodes.Ldarg_0);
c.EmitDelegate<Func<bool, RainCycle, bool>>((Check, self) =>
{
    if (self.world.game.rainWorld.ExpeditionMode)
    {
        return false;
    }
    else
    {
        return self.world.game.GetStorySession.saveState.miscWorldSaveData.pebblesEnergyTaken;
    }
});
*/




//Pearl backup

//private static List<DataPearl.AbstractDataPearl.DataPearlType>? decipheredPearlsInTrans;
//private static List<DataPearl.AbstractDataPearl.DataPearlType>? decipheredDMPearlsInTrans;
//private static List<DataPearl.AbstractDataPearl.DataPearlType>? decipheredFuturePearlsInTrans;
//private static List<DataPearl.AbstractDataPearl.DataPearlType>? decipheredPebblesPearlsInTrans;
//private static List<ChatlogData.ChatlogID>? discoveredBroadcastsInTrans;



//decipheredPearlsInTrans = new List<DataPearl.AbstractDataPearl.DataPearlType>(self.miscProgressionData.decipheredPearls);
//decipheredDMPearlsInTrans = new List<DataPearl.AbstractDataPearl.DataPearlType>(self.miscProgressionData.decipheredDMPearls);
//decipheredFuturePearlsInTrans = new List<DataPearl.AbstractDataPearl.DataPearlType>(self.miscProgressionData.decipheredFuturePearls);
//decipheredPebblesPearlsInTrans = new List<DataPearl.AbstractDataPearl.DataPearlType>(self.miscProgressionData.decipheredPebblesPearls);
//discoveredBroadcastsInTrans = new List<ChatlogData.ChatlogID>(self.miscProgressionData.discoveredBroadcasts);




/*if (self.miscProgressionData.discoveredBroadcasts.Count < discoveredBroadcastsInTrans.Count)
{
    self.miscProgressionData.discoveredBroadcasts = new List<ChatlogData.ChatlogID>(discoveredBroadcastsInTrans);
}
//Debug.Log("Post Tokens Amount B:" + self.miscProgressionData.sandboxTokens.Count+" G:"+self.miscProgressionData.levelTokens.Count);

self.miscProgressionData.decipheredFuturePearls = new List<DataPearl.AbstractDataPearl.DataPearlType>(decipheredFuturePearlsInTrans);
//Pearl retrofitting
decipheredPearlsInTrans.AddRange(self.miscProgressionData.decipheredPearls);
self.miscProgressionData.decipheredPearls = decipheredPearlsInTrans.Distinct().ToList();

decipheredDMPearlsInTrans.AddRange(self.miscProgressionData.decipheredDMPearls);
self.miscProgressionData.decipheredDMPearls = decipheredDMPearlsInTrans.Distinct().ToList();

decipheredPebblesPearlsInTrans.AddRange(self.miscProgressionData.decipheredPebblesPearls);
self.miscProgressionData.decipheredPebblesPearls = decipheredPebblesPearlsInTrans.Distinct().ToList();*/