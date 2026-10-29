using System.Collections.Generic;

/// <summary>
/// オブジェクトプール
/// 
/// オブジェクトのプールを行います
/// </summary>
public class ObjectPool<T> where T : APooledObject
{
    #region State
    private Stack<APooledObject> objectPool; // プール
    private IPooledObjectFactory objectFactory; // オブジェクトのファクトリー
    #endregion

    /// @brief オブジェクトのプールを作成してファクトリーを保持します
    public ObjectPool(IPooledObjectFactory factory)
    {
        objectPool = new Stack<APooledObject>();
        objectFactory = factory;
    }

    /// @brief プールの初期設定を行います
    public void PoolSetUp(uint initPoolSize)
    {
        if(objectFactory == null)
        {
            return;
        }

        for(int i = 0; i < initPoolSize; i++)
        {
            T instance = ObjectInstantiate();

            // 生成後のnull確認のため参照のみを判定します
            if(instance is null)
            {
                return;
            }

            instance.gameObject.SetActive(false);
            objectPool.Push(instance);
        }
    }

    /// @brief プールからオブジェクトの取り出しを行います
    public APooledObject GetFromPool()
    {
        if(objectFactory == null)
        {
            return null;
        }

        // プールにオブジェクトが入っていない場合
        // 緊急措置として新しく生成を行います
        if(objectPool.Count < 1)
        {
            T newInstance = ObjectInstantiate();
            if(newInstance is null)
            {
                return null;
            }

            return newInstance;
        }

        T nextInstance = objectPool.Pop() as T;
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    /// @brief オブジェクトを生成します
    private T ObjectInstantiate()
    {
        T instance = objectFactory.Create() as T;
        if (instance == null) return null;

        // 生成後、返還処理をイベントに追加
        instance.SetPoolAction(ReturnToPool);
        return instance;
    }

    /// @biref オブジェクトをプールに返還する処理を行います
    public void ReturnToPool(APooledObject pooledObject)
    {
        objectPool.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
