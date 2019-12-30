using PageBuilder.Data;
using PageBuilder.Infrastructure.EventMessage;
using PageBuilder.View;
using System;
using System.Collections.Generic;

namespace PageBuilder.Infrastructure.Factory
{
    public class EventMessageFactory
    {
        //public IEventMessageBase createEventMessage(EditActionType EAT)
        //{
        //    IEventMessageBase result;
        //    //event Notify  
        //    switch (EAT)
        //    {
        //        case EditActionType.MoveSpread:
        //            result = 
        //            break;
        //        case EditActionType.DeleteGraphic:
        //            break;
        //        case EditActionType.DeleteLayer:
        //            break;
        //        case EditActionType.DeleteSpread:
        //            break;
        //        case EditActionType.InsertLayer:
        //            break;
        //        case EditActionType.PasteGraphic:
        //            break;
        //        case EditActionType.InsertGraphic:
        //            break;
        //    }
        //    return result;
        //}
    }
}

namespace PageBuilder.Infrastructure.EventMessage
{
    public interface IEventMessageBase { } 
    /// <summary>
    /// 변수 모델이 삭제되었을 때 이벤트    
    /// 발송자 : VariableModelListHandler -> 수신자 : VariableShow (창)
    /// </summary>
    public class variableModelListShouldBeChangedEvent : IEventMessageBase
    {
        // 참조할 필드 이름
        static public string dicFieldName { get; } = "RemovedGraphics";
        // 필드가 가지고 있어야할 데이터 타입 
        public List<Graphic> dicFielValueType;
    }

    public class PageDeleteEvent : IEventMessageBase
    {
        static public string dicPageId { get; } = "RemovedPageId";
        public Guid Id;
    }

    public class PageCreatedEvent : IEventMessageBase
    {
        static public string dicPageId { get; } = "CreatePageId";
        public (Guid CreatedPageId, int CreatedIndex) msg;
    }

    public class PageReorderEvent : IEventMessageBase
    {
    }

}
