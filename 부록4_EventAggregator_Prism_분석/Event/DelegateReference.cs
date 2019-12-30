using System;
using System.Reflection;

namespace PageBuilder.Infrastructure.Event
{
    /// <summary>
    /// <see see="Delegate"/>의 레퍼런스를 표현합니다. 
    /// </summary>
    public interface IDelegateReference
    {
        /// <summary>
        /// 참조하고 있는 <see cref="Delegate"/>객체를 가져옵니다.
        /// </summary>
        /// <value><see cref="Delegate"/> 객체를 반환합니다. 사용가능하지 않다면 <see langword="null"/>을 반환합니다.</value>
        Delegate Target { get; }
    }

    public class DelegateReference : IDelegateReference
    {
        private readonly Delegate _delegate;
        private readonly WeakReference _weakReference;
        private readonly MethodInfo _method;
        private readonly Type _delegateType;

        /// <summary>
        /// <see cref="DelegateReference"/>의 개체를 초기화합니다.
        /// </summary>
        /// <param name="delegate">참조하기 위해 만들 대상인 원본 <see cref="Delegate"/>입니다.</param>
        /// <param name="keepReferenceAlive">만약 <see langword="false"/>이면 가비지 콜렉트 되게끔, 대리자에 Weak reference를 하고 그렇지 않다면 타겟에 대해 강한 참조를 유지합니다.</param>
        /// <exception cref="ArgumentNullException">만약 <paramref name="delegate"/>가 <see cref="Delegate"/>에 할당할 수 없다면 익셉션이 발생합니다.</exception>
        public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
        {
            if (@delegate == null)
                throw new ArgumentNullException("delegate");

            if (keepReferenceAlive)
            {
                this._delegate = @delegate;
            }
            else
            {
                _weakReference = new WeakReference(@delegate.Target);
                _method = @delegate.GetMethodInfo();
                _delegateType = @delegate.GetType();
            }
        }

        /// <summary>
        /// 현재 <see cref="DelegateReference"/>개체가 참조하는 <see cref="Delegate"/>(Target)을 가져옵니다.  
        /// </summary>
        /// <value>현재 <see cref="DelegateReference"/>개체가 참조하던 개체가 가비지 콜렉트가 되었다면 <see langword="null"/>을 반환합니다; 그렇지 않다면, 현재의 <see cref="DelegateReference">가 참조하는 <see cref="Delegate"/>의 참조를 반환합니다. </value>
        public Delegate Target
        {
            get
            {
                if (_delegate != null) return _delegate;
                return TryGetDelegate();
            }

        }

        /// <summary>
        /// <see cref="DelegateReference"/>이 참조하는 <see cref="Delegate"/> (Target)이 다른 <see cref="Delegate"/> 와 같은지 확인합니다.
        /// <paramref name="delegate"/>와 <see cref="Target"/>을 비교하는 것과 같습니다. 
        /// </summary>
        /// <param name="delegate">비교할 다른 delegate 입니다.</param>
        /// <returns>만약 <paramref name="delegate"/>와 현재 개체가 같다면 참을 반환합니다.</returns>
        public bool TargetEquals(Delegate @delegate)
        {
            if (_delegate != null) return _delegate == @delegate;
            if (@delegate == null) return !_method.IsStatic && !_weakReference.IsAlive;
            return _weakReference.Target == @delegate.Target && Equals(_method, @delegate.GetMethodInfo());
        }

        private Delegate TryGetDelegate()
        {
            if (_method.IsStatic)
            {
                return _method.CreateDelegate(_delegateType, null);
            }
            object target = _weakReference.Target;
            if (target != null)
            {
                return _method.CreateDelegate(_delegateType, target);
            }
            return null;
        }
    }
}
