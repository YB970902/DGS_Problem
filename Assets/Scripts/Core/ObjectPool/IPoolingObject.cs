


namespace DGS.Core.ObjectPool
{
    /// <summary>
    /// 오브젝트 풀에 넣기 위해선 꼭 상속받아야 하는 인터페이스
    /// </summary>
    public interface IPoolingObject
    {
        /// <summary>
        /// 생성될 때 1회 호출되는 함수. 자신이 들어갈 풀을 파라미터로 받아야한다. 
        /// </summary>
        public void Init(IObjectPool objectPool);
        /// <summary>
        /// 풀 안에 있는 상태인지 여부
        /// </summary>
        public bool IsInPool { get; }
        /// <summary>
        /// 오브젝트를 넣기 직전 호출되는 함수. 최초로 생성되었을 때 Init 이후로도 1회 호출된다.
        /// 사용이 끝난 변수등을 초기화하는 로직이 들어간다.
        /// </summary>
        public void Sleep();
        /// <summary>
        /// 오브젝트를 꺼내고 나서 가장 먼저 호출되는 함수.
        /// 사용을 준비하는 기능이 들어간다.
        /// </summary>
        public void WakeUp();
        /// <summary>
        /// 본래의 오브젝트 풀로 돌아가는 함수.
        /// </summary>
        public void ReturnToPool();
    }
}