
interface IJsonI
{//.json形式での入力をサポートするものに付与するインターフェース. 実際にはUtility.JsonImport<T>を使う
    bool JsonImport(string path, string name);//pathディレクトリの[naem].jsonを読み込み,自分自身に適用 成功すればtrue 失敗すればfalse
}

interface IJsonO
{//.json形式での出力をサポートするものに付与するインターフェース. 実際にはUtility.JsonExport<T>を使う
    bool JsonExport(string path, string name, bool overwrite);//pathディレクトリに[naem].jsonとして保存 成功すればtrue 失敗すればfalse
}

interface IJsonIO : IJsonI, IJsonO
{/*.json形式での入出力をサポートするものに付与するインターフェース.*/}

interface IJsonSavable : IJsonO
{//.json形式でセーブするものに付与するインターフェース.
    bool SaveAs(string savename, bool overwrite);//セーブ名をsavenameとしてデータを出力. overwriteで上書きの許可,不許可を制御. 成功すればtrue 失敗すればLogAssertionで警告を表示しfalse
}

interface IJsonLoadable : IJsonI
{//.json形式でロードするものに付与するインターフェース.
    bool LoadFrom(string savename);//セーブ名savenameからデータを入力. overwriteで上書きの許可,不許可を制御. 成功すればtrue データが存在しないなどで失敗すればLogAssertionで警告を表示しfalse
}

interface IJsonSaveLoadable : IJsonSavable, IJsonLoadable
{/*.json形式でセーブ,ロードするものに付与するインターフェース.*/}

interface IJsonTemporarySavable : IJsonO
{//.json形式でセーブするものに付与するインターフェース.
    bool SaveTemporary();//Temporaryにデータを出力.既に存在すれば上書き. 成功すればtrue 失敗すればLogAssertionで警告を表示しfalse
}

interface IJsonTemporaryLoadable : IJsonI
{//.json形式でロードするものに付与するインターフェース.
    bool LoadTemporary();//Temporaryからデータを入力. 成功すればtrue データが存在しないなどで失敗すればLogAssertionで警告を表示しfalse
}

interface IJsonTemporarySaveLoadable : IJsonTemporarySavable, IJsonTemporaryLoadable
{/*.json形式で一時的にセーブ,ロードするものに付与するインターフェース.*/}

interface IJsonInitializable : IJsonI
{//初期にロードするデータ(Initial.json)を持っているものに付与するインターフェース.
    void Initialize(string name);//nameのInitial.jsonのロード
}

interface IVisibleObject{//見えるもの(マップチップなど)に付与するインターフェース.
    void Refresh();//自分の持つデータからSpriteやTransform等を設定する.
}
