using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//!< Unity, NGUI 사용
//!< 스크롤 세팅 시 선택한 인덱스로 좌, 우, 중앙 정렬
/*
 * [O--] : 좌
 * [-O-] : 중앙
 * [--O] : 우
*/
public class ScrollAlignment
{
	enum Pivot
	{
		Left = 1,
		Middle = 2,
		Right,
	}

	/// <summary>
	/// 스크롤 총 길이를 0~1로 보고 해당 인덱스의 노멀라이즈 값을 찾고 스크롤에서 보이는 범위를 넘어갔다면 필요한 만큼 이동시켜준다.
	/// 양 끝 값이라면 이동 시키지 않는다.
	/// </summary>
	/// <param name="pivot">좌, 우, 중앙 정렬 선택</param>
	/// <param name="sv">NGUI 스크롤</param>
	/// <param name="grid">NGUI 그리드</param>
	/// <param name="itemWidth">스크롤 자식으로 들어가는 아이템 한개의 width</param>
	/// <param name="maxCount">자식 아이템 최대 갯수</param>
	/// <param name="index">선택한 인덱스</param>
	private void MoveScroll( Pivot pivot, UIScrollView sv, UIGrid grid, int itemWidth, int maxCount, int index )
	{
		if( sv != null )
		{
			sv.ResetPosition();

			float maxSize = grid.cellWidth * maxCount;
			float outSize = sv.panel.width / maxSize; //!< 아웃되는 범위
			float halfOutSize = outSize / 2; //!< 반쪽 ) 아웃되는 범위
			float itemX = 0;
			float normalize = 0;
			float moveX = 0.0f;

			switch( pivot )
			{
				case Pivot.Left:
					itemX = grid.cellWidth * index;
					break;
				case Pivot.Middle:
					itemX = grid.cellWidth * index + ( itemWidth / 2 ); //!< 아이템 중심 중간으로 변경
					break;
				case Pivot.Right:
					itemX = grid.cellWidth * index + itemWidth; //!< 아이템 중심 우측으로 변경
					break;
				default:
					break;
			}

			if( itemX <= 0 )
				itemX = 0;

			normalize = itemX / maxSize;

			switch( pivot )
			{
				case Pivot.Left:
					{
						if( normalize <= ( 1 - outSize ) )
						{
							moveX = normalize * maxSize; //!< 왼
						}
						else
						{
							if( normalize > outSize )
								moveX = maxSize * ( 1 - outSize );
						}
					}
					break;
				case Pivot.Middle:
					{
						if( halfOutSize <= normalize && normalize <= ( 1 - halfOutSize ) )
						{
							moveX = ( normalize - halfOutSize ) * maxSize; //!< 중앙
						}
						else
						{
							if( normalize > halfOutSize )
								moveX = maxSize * ( 1 - outSize );
						}
					}
					break;
				case Pivot.Right:
					{
						if( outSize <= normalize )
						{
							moveX = ( normalize - outSize ) * maxSize; //!< 오른쪽
						}
						else
						{
							if( normalize > outSize )
								moveX = maxSize * ( 1 - outSize );
						}
					}
					break;
				default:
					break;
			}

			sv.MoveRelative( new Vector3( -moveX, 0f, 0f ) );
		}
	}
}